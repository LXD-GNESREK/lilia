// src/Domain/Entities/BaseUnit.cs

using Lilia.Domain.Interfaces;

namespace Lilia.Domain.Entities
{
    public abstract class BaseUnit : IForceElement
    {
        public string Name => Blueprint != null ? Blueprint.Name : "Unknown Unit";
        public Guid InternalID { get; private set; } = Guid.NewGuid();
        public Guid RegistryUnitID { get; private set; }
        public RegistryUnit Blueprint { get; private set; } = null!;
        public Guid? OrganisationID { get; private set; }
        public Organisation? Organisation { get; private set; }
        public abstract string Prefix { get; }
        public string RegistryNumber { get; private set;}
        public string DisplayTag => $"{Name} ({Prefix}-{RegistryNumber})";

        protected BaseUnit() {}

        public BaseUnit(Guid registryUnitID, string registryNumber)
        {
            if(string.IsNullOrWhiteSpace(registryNumber)) throw new ArgumentException("Registry number cannot be null or empty.", nameof(registryNumber));

            RegistryUnitID = registryUnitID;
            RegistryNumber = registryNumber;
        }

        public long Total()
        {
            return Blueprint != null ? Blueprint.Cost : 0;
        }

        public abstract void Display(int indentLevel);

        public void DisplaySimple(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}- {DisplayTag}");
        }

        internal void SetOrganisation(Organisation? organisation)
        {
            OrganisationID = organisation?.InternalID;
            Organisation = organisation;
        }

        protected void SetBlueprint(RegistryUnit blueprint)
        {
            Blueprint = blueprint;
        }
    }
}