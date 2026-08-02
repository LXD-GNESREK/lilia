using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lilia.Presentation.Preconditions
{
    public class RequireStaffAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckRequirementsAsync(
            IInteractionContext context,
            ICommandInfo command,
            IServiceProvider services)
        {
            var config = services.GetRequiredService<IConfiguration>();

            ulong staffRoleID = config.GetValue<ulong>("Roles:AdminStaffRoleID");
            ulong shopReviewRoleID = config.GetValue<ulong>("Roles:ShopReviewRoleID");

            if(context.User is SocketGuildUser guildUser)
            {
                if(guildUser.Roles.Any(r => r.Id == staffRoleID || r.Id == shopReviewRoleID))
                {
                    return PreconditionResult.FromSuccess();
                }
            }

            return PreconditionResult.FromError("You lack the required staff role to execute this command.");
        }
    }
}