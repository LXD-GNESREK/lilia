// src/Domain/Entities/Units/Starfighter.cs

using Lilia.Domain.Enums;

namespace Lilia.Domain.Entities.Units
{
    public class Starfighter : BaseUnit
    {
        public double? Length { get; private set; }
        public double? Width { get; private set; }
        public double? Height { get; private set; }
        public override string Prefix => "S";
        public int? SpaceSpeed { get; private set; }
        public int? AtmosphericSpeed { get; private set; }
        public double? HyperdriveRating { get; private set; }
        public RelativeRating Shields { get; private set; }
        public RelativeRating Hull { get; private set; }
        public RelativeRating Maneuverability { get; private set; }
        public int? CargoCapacity { get; private set; }
        public string? Consumables { get; private set; }

        internal Starfighter(string name, int price, int modslots, string registryNumber, double? length, double? width, double? height, int? spaceSpeed, int? atmosphericSpeed, double? hyperdriveRating, RelativeRating shields, RelativeRating hull, RelativeRating maneuverability, int? cargoCapacity, string? consumables) : base(name, price, modslots, registryNumber)
        {
            if (length.HasValue && length.Value < 0) throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
            if (width.HasValue && width.Value < 0) throw new ArgumentOutOfRangeException(nameof(width), "Width cannot be negative.");
            if (height.HasValue && height.Value < 0) throw new ArgumentOutOfRangeException(nameof(height), "Height cannot be negative.");
            if (spaceSpeed.HasValue && spaceSpeed.Value < 0) throw new ArgumentOutOfRangeException(nameof(spaceSpeed), "Speed cannot be negative.");
            if (atmosphericSpeed.HasValue && atmosphericSpeed.Value < 0) throw new ArgumentOutOfRangeException(nameof(atmosphericSpeed), "Speed cannot be negative.");
            if (cargoCapacity.HasValue && cargoCapacity.Value < 0) throw new ArgumentOutOfRangeException(nameof(cargoCapacity), "Cargo capacity cannot be negative.");

            Length = length;
            Width = width;
            Height = height;
            SpaceSpeed = spaceSpeed;
            AtmosphericSpeed = atmosphericSpeed;
            HyperdriveRating = hyperdriveRating;
            Shields = shields;
            Hull = hull;
            Maneuverability = maneuverability;
            CargoCapacity = cargoCapacity;
            Consumables = consumables;
        }

        public override void Display(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}{DisplayTag}");
            Console.WriteLine($"{indent}  Type: Starfighter");
            Console.WriteLine($"{indent}  Length: {(Length.HasValue ? $"{Length.Value} m" : "Unknown")}");
            Console.WriteLine($"{indent}  Width: {(Width.HasValue ? $"{Width.Value} m" : "Unknown")}");
            Console.WriteLine($"{indent}  Height: {(Height.HasValue ? $"{Height.Value} m" : "Unknown")}");
            Console.WriteLine($"{indent}  Space Speed: {(SpaceSpeed.HasValue ? $"{SpaceSpeed.Value} MGLT" : "Unknown")}");
            Console.WriteLine($"{indent}  Atmospheric Speed: {(AtmosphericSpeed.HasValue ? $"{AtmosphericSpeed.Value} km/h" : "Unknown")}");
            Console.WriteLine($"{indent}  Hyperdrive Rating: {(HyperdriveRating.HasValue ? $"Class {HyperdriveRating.Value}" : "Unknown")}");
            Console.WriteLine($"{indent}  Shields: {Shields}");
            Console.WriteLine($"{indent}  Hull: {Hull}");
            Console.WriteLine($"{indent}  Maneuverability: {Maneuverability}");
            Console.WriteLine($"{indent}  Cargo Capacity: {(CargoCapacity.HasValue ? $"{CargoCapacity.Value} kg" : "Unknown")}");
            Console.WriteLine($"{indent}  Consumables: {(string.IsNullOrEmpty(Consumables) ? "Unknown" : Consumables)}");
        }
    }
}