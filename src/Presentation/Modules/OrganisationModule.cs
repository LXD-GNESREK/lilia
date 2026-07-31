using System.Text;
using System.Transactions;
using Discord.Interactions;
using Lilia.Domain.Entities;
using Lilia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lilia.Presentation.Modules
{
    [Group("org", "Commands to manage your organisations.")]
    public class OrganisationModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly LiliaDBContext _dbContext;

        public OrganisationModule(LiliaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [SlashCommand("create", "Create a new organisation or formation.")]
        public async Task CreateOrganisation(
            [Summary("name", "The full name of the organisation (e.g., 1st Legion).")] string name,
            [Summary("short_code", "A unique identifier for this organisation (e.g., 1LEG).")] string shortCode,
            [Summary("parent_code", "Optional: The short code of the parent organisation")] string? parentCode = null
        )
        {
            await DeferAsync();

            shortCode = shortCode.ToUpperInvariant();
            var player = await GetOrCreatePlayerAsync(Context.User.Id);

            bool exists = await _dbContext.Organisations.AnyAsync(o => o.PlayerDiscordID == player.DiscordID && o.ShortCode == shortCode);

            if(exists)
            {
                await FollowupAsync($"You already have an organisation with the shortcode **{shortCode}**");
                return;
            }

            var newOrg = new Organisation(name, shortCode, player.DiscordID);

            if(!string.IsNullOrWhiteSpace(parentCode))
            {
                parentCode = parentCode.ToUpperInvariant();
                var parentOrg = await _dbContext.Organisations.FirstOrDefaultAsync(o => o.PlayerDiscordID == player.DiscordID && o.ShortCode == parentCode);

                if(parentOrg == null)
                {
                    await FollowupAsync($"Could not find any parent organisation with the short code **{parentCode}**.");
                    return;
                }

                parentOrg.Add(newOrg);
            }

            _dbContext.Organisations.Add(newOrg);
            await _dbContext.SaveChangesAsync();
            await FollowupAsync($"Successfully created **{name}** [{shortCode}].");
        }

        [SlashCommand("view", "View the details and hierarchy of an organisation.")]
        public async Task ViewOrganisationAsync(
            [Summary("short_code", "The short code of the organisation to view.")] string shortCode)
        {
            await DeferAsync();

            shortCode = shortCode.ToUpperInvariant();

            var org = await _dbContext.Organisations
                                            .Include(o => o.SubOrganisations)
                                            .FirstOrDefaultAsync(o => o.PlayerDiscordID == Context.User.Id && o.ShortCode == shortCode);
            
            if(org == null)
            {
                await FollowupAsync($"Could not find an organisation with the short code **{shortCode}**.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"**{org.Name}** [{org.ShortCode}]");
            sb.AppendLine("---");

            if(org.SubOrganisations != null && org.SubOrganisations.Count > 0)
            {
                sb.AppendLine("**Sub-Organisations:**");
                foreach(var subOrg in org.SubOrganisations)
                {
                    sb.AppendLine($"- {subOrg.Name} [{subOrg.ShortCode}]");
                }
            }
            else
            {
                sb.AppendLine("*No Sub-Organisations.*");
            }

            await FollowupAsync(sb.ToString());
        }

        private async Task<Player> GetOrCreatePlayerAsync(ulong discordID)
        {
            var player = await _dbContext.Players.FirstOrDefaultAsync(p => p.DiscordID == discordID);

            if(player == null)
            {
                player = new Player(discordID);
                _dbContext.Players.Add(player);
                await _dbContext.SaveChangesAsync();
            }

            return player;
        }
    }
}