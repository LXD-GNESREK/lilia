// src/Presentation/Utilities/WeaponTypeAutocompleteHandler.cs

using Discord;
using Discord.Interactions;
using Lilia.Domain.Enums;
using System.Text.RegularExpressions;

namespace Lilia.Presentation.Autocomplete
{
    public class WeaponTypeAutocompleteHandler : AutocompleteHandler
    {
        public override Task<AutocompletionResult> GenerateSuggestionsAsync(
            IInteractionContext context, 
            IAutocompleteInteraction autocompleteInteraction, 
            IParameterInfo parameter, 
            IServiceProvider services)
        {
            string userInput = autocompleteInteraction.Data.Current.Value.ToString() ?? string.Empty;

            var suggestions = Enum.GetValues<WeaponType>()
                .Select(w => new 
                { 
                    RawValue = w.ToString(), 
                    DisplayName = Regex.Replace(w.ToString(), "(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])", " $1$2").Trim()
                })
                .Where(w => string.IsNullOrEmpty(userInput) || 
                            w.DisplayName.Contains(userInput, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .Select(w => new AutocompleteResult(w.DisplayName, w.RawValue));

            return Task.FromResult(AutocompletionResult.FromSuccess(suggestions));
        }
    }
}