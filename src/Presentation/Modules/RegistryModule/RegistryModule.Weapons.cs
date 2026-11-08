// src/Presentation/Modules/RegistryModule/RegistryModule.Weapons.cs

using Discord.Interactions;
using Lilia.Domain.Entities.RegistryStats;
using Lilia.Domain.Enums;
using Lilia.Presentation.Autocomplete;
using Lilia.Presentation.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lilia.Presentation.Modules
{
    public partial class RegistryModule
    {
        [SlashCommand("add-weapon", "Add a weapon mount to a unit's armament.")]
        public async Task AddWeaponAsync(
            [Summary("unit", "The unit to modify")] [Autocomplete(typeof(RegistryUnitAutocompleteHandler))] string unitIdString,
            [Summary("weapon", "The type of weapon")] [Autocomplete(typeof(WeaponTypeAutocompleteHandler))] string weaponTypeString,
            [Summary("quantity", "Number of weapons in this mount")] int quantity,
            [Summary("config", "e.g., Twin-linked, Turret, Aft (Optional)")] string? config = null)
        {
            await DeferAsync();

            if (quantity <= 0)
            {
                await FollowupAsync("Quantity must be greater than zero.");
                return;
            }

            if (!Guid.TryParse(unitIdString, out Guid unitId))
            {
                await FollowupAsync("Invalid unit selection.");
                return;
            }

            if (!Enum.TryParse<WeaponType>(weaponTypeString, out var weaponType))
            {
                await FollowupAsync("Invalid weapon selection. Please use the autocomplete menu.");
                return;
            }

            var unit = await _dbContext.RegistryUnits.FirstOrDefaultAsync(u => u.ID == unitId);
            if (unit == null)
            {
                await FollowupAsync("Unit not found in the registry.");
                return;
            }

            switch (unit.Category)
            {
                case UnitCategory.Starfighter:
                    await ProcessStarfighterWeaponAsync(unit, weaponType, quantity, config);
                    break;
                default:
                    await FollowupAsync($"Weapon mounts are not currently supported for category: {unit.Category}");
                    break;
            }
        }

        [SlashCommand("remove-weapon", "Remove all weapon mounts of a specific type from a unit.")]
        public async Task RemoveWeaponAsync(
            [Summary("unit", "The unit to modify")] [Autocomplete(typeof(RegistryUnitAutocompleteHandler))] string unitIdString,
            [Summary("weapon", "The type of weapon to remove")] [Autocomplete(typeof(WeaponTypeAutocompleteHandler))] string weaponTypeString)
        {
            await DeferAsync();

            if (!Guid.TryParse(unitIdString, out Guid unitId))
            {
                await FollowupAsync("Invalid unit selection.");
                return;
            }

            if (!Enum.TryParse<WeaponType>(weaponTypeString, out var weaponType))
            {
                await FollowupAsync("Invalid weapon selection. Please use the autocomplete menu.");
                return;
            }

            var unit = await _dbContext.RegistryUnits.FirstOrDefaultAsync(u => u.ID == unitId);
            if (unit == null)
            {
                await FollowupAsync("Unit not found in the registry.");
                return;
            }

            switch (unit.Category)
            {
                case UnitCategory.Starfighter:
                    await ProcessStarfighterWeaponRemovalAsync(unit, weaponType);
                    break;
                default:
                    await FollowupAsync($"Weapon mounts are not currently supported for category: {unit.Category}");
                    break;
            }
        }

        private async Task ProcessStarfighterWeaponRemovalAsync(Domain.Entities.RegistryUnit unit, WeaponType weaponType)
        {
            if (!unit.HasStats || string.IsNullOrWhiteSpace(unit.StatDataJson))
            {
                await FollowupAsync($"**{unit.Name}** has no stats or weapons configured yet.");
                return;
            }

            StarfighterStats stats;
            try
            {
                stats = JsonSerializer.Deserialize<StarfighterStats>(unit.StatDataJson) ?? new StarfighterStats();
            }
            catch (JsonException)
            {
                await FollowupAsync("Failed to parse the unit's stats.");
                return;
            }

            int removedMounts = stats.Armament.RemoveAll(m => m.WeaponType == weaponType);

            string prettyWeaponName = System.Text.RegularExpressions.Regex.Replace(weaponType.ToString(), "(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])", " $1$2").Trim();

            if (removedMounts == 0)
            {
                await FollowupAsync($"**{unit.Name}** does not currently have any **{prettyWeaponName}** mounts equipped.");
                return;
            }

            string updatedJson = JsonSerializer.Serialize(stats);
            unit.UpdateStats(updatedJson);

            await _dbContext.SaveChangesAsync();

            await FollowupAsync($"Successfully removed {removedMounts} **{prettyWeaponName}** mount(s) from **{unit.Name}**.");
        }

        private async Task ProcessStarfighterWeaponAsync(Domain.Entities.RegistryUnit unit, WeaponType weaponType, int quantity, string? config)
        {
            StarfighterStats stats;
            if (unit.HasStats && !string.IsNullOrWhiteSpace(unit.StatDataJson))
            {
                try
                {
                    stats = JsonSerializer.Deserialize<StarfighterStats>(unit.StatDataJson) ?? new StarfighterStats();
                }
                catch (JsonException)
                {
                    stats = new StarfighterStats();
                }
            }
            else
            {
                stats = new StarfighterStats();
            }

            var newMount = new WeaponMount
            {
                Quantity = quantity,
                WeaponType = weaponType,
                Configuration = config
            };

            stats.Armament.Add(newMount);

            string updatedJson = JsonSerializer.Serialize(stats);
            unit.UpdateStats(updatedJson);

            await _dbContext.SaveChangesAsync();

            string prettyWeaponName = System.Text.RegularExpressions.Regex.Replace(weaponType.ToString(), "(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])", " $1$2").Trim();
            string configText = string.IsNullOrWhiteSpace(config) ? "" : $" ({config})";
            
            await FollowupAsync($"Successfully added **{quantity} {prettyWeaponName}{configText}** to **{unit.Name}**.");
        }
    }
}