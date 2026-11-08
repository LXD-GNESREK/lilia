// src/Presentation/Modules/RegistryModule/RegistryModule.View.cs

using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Discord;
using Discord.Interactions;
using Lilia.Domain.Entities.RegistryStats;
using Lilia.Domain.Enums;
using Lilia.Presentation.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Lilia.Presentation.Modules
{
    public partial class RegistryModule
    {
        [SlashCommand("view", "View the complete details and stats of a registry unit.")]
        public async Task ViewUnitAsync(
        [Summary("unit", "The unit to view")] [Autocomplete(typeof(RegistryUnitAutocompleteHandler))] string unitIdString)
        {
            await DeferAsync();

            if (!Guid.TryParse(unitIdString, out Guid unitId))
            {
                await FollowupAsync("Invalid selection. Please use the autocomplete menu.");
                return;
            }

            var unit = await _dbContext.RegistryUnits.FirstOrDefaultAsync(u => u.ID == unitId);
            if (unit == null)
            {
                await FollowupAsync("Unit not found in the registry.");
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle(unit.Name)
                .WithColor(Color.Blue)
                .AddField("Category", FormatEnum(unit.Category.ToString()), true)
                .AddField("Cost", $"{unit.Cost:N0} Credits", true)
                .AddField("Modslots", unit.Modslots.ToString(), true);

            if (unit.HasStats && !string.IsNullOrWhiteSpace(unit.StatDataJson))
            {
                switch (unit.Category)
                {
                    case UnitCategory.Starfighter:
                        BuildStarfighterEmbed(embed, unit.StatDataJson);
                        break;
                    default:
                        embed.AddField("Stats", $"Stats exist but viewing is not yet supported for category: {unit.Category}");
                        break;
                }
            }
            else
            {
                embed.AddField("Stats", "*No extended stats have been inputted for this unit yet.*");
            }

            await FollowupAsync(embed: embed.Build());
        }

        private void BuildStarfighterEmbed(EmbedBuilder embed, string json)
        {
            StarfighterStats stats;
            try
            {
                stats = JsonSerializer.Deserialize<StarfighterStats>(json) ?? new StarfighterStats();
            }
            catch
            {
                embed.AddField("Error", "Failed to parse unit stats.");
                return;
            }

            string shieldText = FormatRating(stats.ShieldRating, stats.ShieldRatingOverride);
            string hullText = FormatRating(stats.HullRating, stats.HullRatingOverride);
            embed.AddField("Survivability", $"**Shields:** {shieldText}\n**Hull:** {hullText}", false);

            string hyperdrive = stats.HasHyperdrive ? $"Class {stats.HyperdriveRating}" : "None";
            embed.AddField("Performance", 
                $"**Space:** {stats.SpaceSpeed} MGLT\n" +
                $"**Atmosphere:** {stats.AtmosphericSpeed} km/h\n" +
                $"**Hyperdrive:** {hyperdrive}", true);

            embed.AddField("Dimensions & Capacity", 
                $"**Length:** {stats.Length}m\n" +
                $"**Width:**  {stats.Width}m\n" +
                $"**Height:** {stats.Height}m\n" +
                $"**Passengers:** {stats.PassengerCapacity}\n" +
                $"**Cargo:** {stats.CargoCapacity} tons\n" +
                $"**Consumables:** {stats.Consumables ?? "N/A"}", true);

            if (stats.Armament.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var mount in stats.Armament)
                {
                    string prettyWeapon = FormatEnum(mount.WeaponType.ToString());
                    string config = string.IsNullOrWhiteSpace(mount.Configuration) ? "" : $" ({mount.Configuration})";
                    sb.AppendLine($"- {mount.Quantity}x {prettyWeapon}{config}");
                }
                embed.AddField("Armament", sb.ToString(), false);
            }
            else
            {
                embed.AddField("Armament", "*Unarmed*", false);
            }
        }

        private string FormatEnum(string enumString)
        {
            return Regex.Replace(enumString, "(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])", " $1$2").Trim();
        }

        // Helper to format Rating + Optional Scale Override
        private string FormatRating(RelativeRating rating, RatingScale? overrideScale)
        {
            string ratingDisplay = FormatEnum(rating.ToString());
            if (overrideScale.HasValue)
            {
                return $"{ratingDisplay} ({FormatEnum(overrideScale.Value.ToString())} scale)";
            }
            return ratingDisplay;
        }
    }
}