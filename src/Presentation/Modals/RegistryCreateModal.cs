using Discord.Interactions;

namespace Lilia.Presentation.Modals
{
    public class RegistryCreateModal : IModal
    {
        public string Title => "Create New Registry Unit";

        [InputLabel("Unit Name")]
        [ModalTextInput("unit_name", placeholder: "e.g., TIE/LN Starfighter", maxLength: 100)]
        public required string Name { get; set; }

        [InputLabel("Cost")]
        [ModalTextInput("unit_cost", placeholder: "e.g., 100,000")]
        public required string Cost { get; set; }

        [InputLabel("Modslots")]
        [ModalTextInput("unit_modslots", placeholder: "e.g., 2")]
        public required string Modslots { get; set; }

        [InputLabel("Category")]
        [ModalTextInput("unit_category", placeholder: "e.g., Starfighter")]
        public required string Category { get; set; }
    }
}