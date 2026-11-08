using Lilia.Domain.Enums;
using Lilia.Domain.Interfaces;

namespace Lilia.Domain.Entities.RegistryStats
{
    public class StarfighterStats : IRegistryStats
    {
        public RelativeRating ShieldRating { get; set; }
        public UnitCategory?  ShieldRatingOverride { get; set; }
        public RelativeRating HullRating { get; set; }
        public UnitCategory? HullRatingOverride { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double SpaceSpeed { get; set; }
        public double AtmosphericSpeed { get; set; }
        public bool HasHyperdrive { get; set; }
        public double? HyperdriveRating { get; set; }
        public int PassengerCapacity { get; set; }
        public double CargoCapacity { get; set; }
        public string? Consumables { get; set; }

        public List<WeaponMount> Armament { get; set; } = new List<WeaponMount>();
    }
}