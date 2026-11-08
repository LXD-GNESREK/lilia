// src/Presentation/Modules/RegistryModule/RegistryModule.Stats.cs

using Discord.Interactions;
using Lilia.Domain.Entities.RegistryStats;
using Lilia.Domain.Enums;
using Lilia.Presentation.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lilia.Presentation.Modules
{
    public partial class RegistryModule
    {
        [SlashCommand("stats-starfighter", "Update scalar stats for a Starfighter.")]
        public async Task UpdateStarfighterStatsAsync(
            [Summary("unit", "The unit to update")] [Autocomplete(typeof(RegistryUnitAutocompleteHandler))] string unitIdString,
            [Summary("shield_rating", "Base shield rating")] RelativeRating? shieldRating = null,
            [Summary("shield_override", "Override the scale category for shields")] UnitCategory? shieldOverride = null,
            [Summary("hull_rating", "Base hull rating")] RelativeRating? hullRating = null,
            [Summary("hull_override", "Override the scale category for hull")] UnitCategory? hullOverride = null,
            [Summary("length", "Length in meters")] double? length = null,
            [Summary("width", "Width in meters")] double? width = null,
            [Summary("height", "Height in meters")] double? height = null,
            [Summary("space_speed", "Space speed in MGLT")] double? spaceSpeed = null,
            [Summary("atmos_speed", "Atmospheric speed in km/h")] double? atmosSpeed = null,
            [Summary("has_hyperdrive", "Does it have a hyperdrive?")] bool? hasHyperdrive = null,
            [Summary("hyperdrive_rating", "Hyperdrive class rating (e.g., 1.0)")] double? hyperdriveRating = null,
            [Summary("passengers", "Passenger capacity")] int? passengers = null,
            [Summary("cargo", "Cargo capacity in metric tons")] double? cargo = null,
            [Summary("consumables", "e.g., '1 week', '2 months'")] string? consumables = null
        )
        {
            await DeferAsync();

            if (!Guid.TryParse(unitIdString, out Guid unitId))
            {
                await FollowupAsync("Invalid selection. Please use the autocomplete menu to select a unit.");
                return;
            }

            var unit = await _dbContext.RegistryUnits.FirstOrDefaultAsync(u => u.ID == unitId);
            if (unit == null)
            {
                await FollowupAsync("That unit could not be found in the registry.");
                return;
            }

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

            if (shieldRating.HasValue) stats.ShieldRating = shieldRating.Value;
            if (shieldOverride.HasValue) stats.ShieldRatingOverride = shieldOverride.Value;
            
            if (hullRating.HasValue) stats.HullRating = hullRating.Value;
            if (hullOverride.HasValue) stats.HullRatingOverride = hullOverride.Value;

            if (length.HasValue) stats.Length = length.Value;
            if (width.HasValue) stats.Width = width.Value;
            if (height.HasValue) stats.Height = height.Value;

            if (spaceSpeed.HasValue) stats.SpaceSpeed = spaceSpeed.Value;
            if (atmosSpeed.HasValue) stats.AtmosphericSpeed = atmosSpeed.Value;

            if (hasHyperdrive.HasValue) stats.HasHyperdrive = hasHyperdrive.Value;
            if (hyperdriveRating.HasValue) stats.HyperdriveRating = hyperdriveRating.Value;

            if (passengers.HasValue) stats.PassengerCapacity = passengers.Value;
            if (cargo.HasValue) stats.CargoCapacity = cargo.Value;
            
            if (consumables != null) stats.Consumables = consumables;

            string updatedJson = JsonSerializer.Serialize(stats);
            unit.UpdateStats(updatedJson);

            await _dbContext.SaveChangesAsync();

            await FollowupAsync($"Successfully updated Starfighter stats for **{unit.Name}**.");
        }
    }
}