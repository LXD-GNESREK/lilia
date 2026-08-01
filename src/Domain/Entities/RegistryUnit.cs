using Lilia.Domain.Enums;

namespace Lilia.Domain.Entities
{
    public class RegistryUnit
    {
        public Guid ID { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public long Cost { get; private set; }
        public int Modslots { get; private set; }
        public UnitCategory Category { get; private set; }
        public bool HasStats { get; private set; }
        public string StatDataJson { get; private set; }

        private RegistryUnit() {}

        public RegistryUnit(string name, long cost, int modslots, UnitCategory category, bool hasStats, string statDataJson =  "{}")
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if(cost < 0) throw new ArgumentOutOfRangeException(nameof(cost), "Cost cannot be negative.");
            if(modslots < 0) throw new ArgumentOutOfRangeException(nameof(modslots), "Modslots cannot be negative.");

            Name = name;
            Cost = cost;
            Modslots = modslots;
            Category = category;
            HasStats = hasStats;
            StatDataJson = statDataJson;
        }

        public void UpdateStats(string statDataJson)
        {
            StatDataJson = statDataJson;
            HasStats = true;
        }
    }
}