using System.Text;
using Discord.Interactions;
using Lilia.Domain.Entities;
using Lilia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lilia.Presentation.Modules
{
    [Group("units", "Commands to manage your Unit Count (UC)")]
    public class UnitModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly LiliaDBContext _dbContext;

        public UnitModule(LiliaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [SlashCommand("view", "View your current unassigned Unit Count (UC)")]
        public async Task ViewUnitsAsync()
        {
            await DeferAsync();

            var player = await GetOrCreatePlayerAsync(Context.User.Id);

            if(player.UnitCount.Units.Count == 0)
            {
                await FollowupAsync("Your Unit Count (UC) is currently empty.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("**Your Assigned Units:**");
            foreach(var kvp in player.UnitCount.Units)
            {
                sb.AppendLine($"- {kvp.Key}: {kvp.Value}");
            }

            await FollowupAsync(sb.ToString());
        }

        [SlashCommand("update", "Update the total quantity of a specific unit in your Unit Count (UC).")]
        public async Task UpdateUnitAsync(
            [Summary("name", "The name of the unit (e.g., TIE/LN Starfighter)")] string unitName,
            [Summary("quantity", "The total amount of this unit you possess")] int quantity)
        {
            await DeferAsync();

            if(quantity < 0)
            {
                await FollowupAsync("Quantity cannot be negative.");
                return;
            }

            var player = await GetOrCreatePlayerAsync(Context.User.Id);

            player.UnitCount.Units[unitName] = quantity;

            await _dbContext.SaveChangesAsync();

            await FollowupAsync($"Successfully updated **{unitName}** to a quantity of **{quantity}**");
        }

        [SlashCommand("add", "Add a quantity of a specific unit to your Unit Count (UC)")]
        public async Task AddUnitAsync(
            [Summary("name", "The name of the unit (e.g., TIE/LN Starfighter)")] string unitName,
            [Summary("quantity", "The total amount of this unit you possess")] int quantity)
        {
            await DeferAsync();

            if(quantity <= 0)
            {
                await FollowupAsync("Quantity cannot be negative or 0.");
                return;
            }

            var player = await GetOrCreatePlayerAsync(Context.User.Id);

            if(player.UnitCount.Units.ContainsKey(unitName))
            {
                player.UnitCount.Units[unitName] += quantity;
            }
            else
            {
                player.UnitCount.Units.Add(unitName, quantity);
            }

            await _dbContext.SaveChangesAsync();
            await FollowupAsync($"Successfully added **{quantity} {unitName}** to your Unit Count (UC)");
        }

        [SlashCommand("remove", "Remove a quantity or all of a specific unit from your Unit Count (UC)")]
        public async Task RemoveUnitAsync(
            [Summary("name", "The name of the unit (e.g., TIE/LN Starfighter)")] string unitName,
            [Summary("quantity", "The total amount of this unit you possess")] string quantity)
        {
            await DeferAsync();

            bool isAll = string.Equals(quantity, "all", StringComparison.OrdinalIgnoreCase);

            if((!isAll && (!int.TryParse(quantity, out int  parsedQuantity) || parsedQuantity <= 0)))
            {
                await FollowupAsync("Quantity needs to be either \"all\" or a positive number.");
                return;
            }

            var player = await GetOrCreatePlayerAsync(Context.User.Id);

            if(!player.UnitCount.Units.ContainsKey(unitName) || player.UnitCount.Units[unitName] == 0)
            {
                await FollowupAsync("You don't own any units of that type, Child.");
                return;
            }

            if(isAll)
            {
                player.UnitCount.Units.Remove(unitName);
                await _dbContext.SaveChangesAsync();
                await FollowupAsync($"Successfully removed **all {unitName}** from your Unit Count (UC)");
                return;
            }

            int removeCount = int.Parse(quantity);

            if(player.UnitCount.Units[unitName] >= removeCount)
            {
                player.UnitCount.Units[unitName] -= removeCount;
                
                if(player.UnitCount.Units[unitName] == 0)
                {
                    player.UnitCount.Units.Remove(unitName);
                }

                await _dbContext.SaveChangesAsync();
                await FollowupAsync($"Successfully removed **{removeCount} {unitName}** from your Unit Count (UC)");
            }
            else
            {
                await FollowupAsync($"You cannot remove more units than you own.");
            }
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