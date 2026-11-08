// src/Presentation/Modules/RegistryModule/RegistryModule.Core.cs

using Lilia.Presentation.Preconditions;
using Lilia.Presentation.Modals;
using Lilia.Domain.Entities;
using Lilia.Infrastructure.Data;
using Lilia.Presentation.Utilities;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using Discord;

namespace Lilia.Presentation.Modules
{
    [RequireStaff]
    [Group("registry", "Manage the global unit registry.")]
    public partial class RegistryModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly LiliaDBContext _dbContext;

        public RegistryModule(LiliaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Slash Commands

        [SlashCommand("create", "Create a new unit in the global registry.")]
        public async Task CreateUnitAsync()
        {
            await RespondWithModalAsync<RegistryCreateModal>("registry_create_modal");
        }

        [SlashCommand("remove", "Remove a unit from the global registry.")]
        public async Task RemoveUnitAsync(
            [Summary("unit", "Search for the unit you want to remove.")]
            [Autocomplete(typeof(RegistryUnitAutocompleteHandler))] string unitIDString)
        {
            await DeferAsync();

            if(!Guid.TryParse(unitIDString, out Guid unitID))
            {
                await FollowupAsync("Invalid selection. Please use the autocomplete menu to select a unit.");
                return;
            }

            var unit = await _dbContext.RegistryUnits.FirstOrDefaultAsync(u => u.ID == unitID);

            if(unit == null)
            {
                await FollowupAsync("That unit could not be found in the registry.");
                return;
            }

            var components = new ComponentBuilder().WithButton("Confirm", $"registry_remove_confirm:{unit.ID}", ButtonStyle.Danger)
                                                   .WithButton("Cancel", $"registry_remove_cancel:{unit.ID}", ButtonStyle.Secondary)
                                                   .Build();
            
            await FollowupAsync($"Are you sure you want to permanently delete **{unit.Name}**?", components: components);
        }

        // Logic for handling submissions of Slash Commands

        [ModalInteraction("registry_create_modal")]
        public async Task OnRegistryCreateModalSubmit(RegistryCreateModal modal)
        {
            await DeferAsync();

            if(!int.TryParse(modal.Cost, out int cost) ||
               !int.TryParse(modal.Modslots, out int modslots) ||
               !int.TryParse(modal.Category, out int categoryInt))
            {
                await FollowupAsync("Invalid input. Please ensure that Cost, Modslots, and Category are valid integers.");
                return;
            }

            var newUnit = new RegistryUnit(modal.Name, cost, modslots, (Domain.Enums.UnitCategory)categoryInt, false);

            bool exists = await _dbContext.RegistryUnits.AnyAsync(u => u.Name.ToLower() == modal.Name.ToLower());

            if(exists)
            {
                await FollowupAsync($"A unit with the name **{modal.Name}** already exists in the registry.");
                return;
            }

            _dbContext.RegistryUnits.Add(newUnit);
            await _dbContext.SaveChangesAsync();

            await FollowupAsync($"Successfully added **{modal.Name}** to the global registry.");
        }

        [ComponentInteraction("registry_remove_confirm:*")]
        public async Task OnRemoveConfirmAsync(string unitIDString)
        {
            await DeferAsync();

            if(!Guid.TryParse(unitIDString, out Guid unitID)) return;

            var unit = await _dbContext.RegistryUnits.FirstOrDefaultAsync(u => u.ID == unitID);

            if(unit != null)
            {
                _dbContext.RegistryUnits.Remove(unit);
                await _dbContext.SaveChangesAsync();

                await ModifyOriginalResponseAsync(msg =>
                {
                    msg.Content = $"Successfully deleted **{unit.Name}** from the global registry.";
                    msg.Components = new ComponentBuilder().Build();
                });
            }
            else
            {
                await ModifyOriginalResponseAsync(msg =>
                {
                    msg.Content = "That unit could not be found in the registry.";
                    msg.Components = new ComponentBuilder().Build();
                });
            }
        }

        [ComponentInteraction("registry_remove_cancel:*")]
        public async Task OnRemoveCancelAsync(string unitIDString)
        {
            await DeferAsync();

            await ModifyOriginalResponseAsync(msg =>
            {
                msg.Content = "Deletion cancelled.";
                msg.Components = new ComponentBuilder().Build();
            });
        }
    }
}