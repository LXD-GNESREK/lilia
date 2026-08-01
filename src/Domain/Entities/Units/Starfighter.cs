// src/Domain/Entities/Units/Starfighter.cs

using Lilia.Domain.Enums;

namespace Lilia.Domain.Entities.Units
{
    public class Starfighter : BaseUnit
    {
        public override string Prefix => "S";

        private Starfighter() {}

        internal Starfighter(Guid registryUnitID, string registryNumber) : base(registryUnitID, registryNumber) {}

        public override void Display(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}{DisplayTag}");
            Console.WriteLine($"{indent}  Type: Starfighter");
            if(Blueprint != null && Blueprint.HasStats)
            {
                Console.WriteLine($"{indent}  [States stored in Blueprint Registry (Unimplemented)]");
            }
        }
    }
}