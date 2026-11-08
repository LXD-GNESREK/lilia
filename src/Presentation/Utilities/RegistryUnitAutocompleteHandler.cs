using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Lilia.Infrastructure.Data;

namespace Lilia.Presentation.Utilities
{
    public class RegistryUnitAutocompleteHandler : AutocompleteHandler
    {
        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(
            IInteractionContext context, 
            IAutocompleteInteraction autocompleteInteraction,
            IParameterInfo parameter,
            IServiceProvider services)
        {
            var dbContext = services.GetRequiredService<LiliaDBContext>();

            string userInput = autocompleteInteraction.Data.Current.Value.ToString() ?? string.Empty;

            var matches = await dbContext.RegistryUnits.Where(u => string.IsNullOrWhiteSpace(userInput) || u.Name.ToLower().Contains(userInput.ToLower()))
                                                       .Take(5)
                                                       .ToListAsync();
            
            var suggestions = matches.Select(u => new AutocompleteResult(u.Name, u.ID.ToString()));
            return AutocompletionResult.FromSuccess(suggestions);
        }
    }
}