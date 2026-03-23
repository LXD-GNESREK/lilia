// src/Domain/Builders/StarfighterBuilder.cs

using Lilia.Domain.Entities.Units;
using Lilia.Domain.Enums;

namespace Lilia.Domain.Builders
{
    public class StarfighterBuilder
    {
        private string _name;
        private int _price;
        private int _modslots;
        private string _registryNumber;
        private RelativeRating _shields = RelativeRating.Unknown;
        private RelativeRating _hull = RelativeRating.Unknown;
        private RelativeRating _maneuverability = RelativeRating.Unknown;
        private double? _length = null;
        private double? _width = null;
        private double? _height = null;
        private int? _spaceSpeed = null;
        private int? _atmosphericSpeed = null;
        private double? _hyperdriveRating = null;
        private int? _cargoCapacity = null;
        private string? _consumables = null;

        public StarfighterBuilder(string name, int price, int modslots, string registryNumber)
        {
            _name = name;
            _price = price;
            _modslots = modslots;
            _registryNumber = registryNumber;
        }

        public StarfighterBuilder WithShields(RelativeRating shields)
        {
            _shields = shields;
            return this;
        }

        public StarfighterBuilder WithHull(RelativeRating hull)
        {
            _hull = hull;
            return this;
        }

        public StarfighterBuilder WithManeuverability(RelativeRating maneuverability)
        {
            _maneuverability = maneuverability;
            return this;
        }

        public StarfighterBuilder WithDimensions(double length, double width, double height)
        {
            _length = length;
            _width = width;
            _height = height;
            return this;
        }

        public StarfighterBuilder WithSpaceSpeed(int spaceSpeed)
        {
            _spaceSpeed = spaceSpeed;
            return this;
        }

        public StarfighterBuilder WithAtmosphericSpeed(int atmosphericSpeed)
        {
            _atmosphericSpeed = atmosphericSpeed;
            return this;
        }

        public StarfighterBuilder WithHyperdriveRating(double hyperdriveRating)
        {
            _hyperdriveRating = hyperdriveRating;
            return this;
        }

        public StarfighterBuilder WithCargoCapacity(int cargoCapacity)
        {
            _cargoCapacity = cargoCapacity;
            return this;
        }

        public StarfighterBuilder WithConsumables(string consumables)
        {
            _consumables = consumables;
            return this;
        }

        public Starfighter Build()
        {
            return new Starfighter(
                                    _name, 
                                    _price, 
                                    _modslots, 
                                    _registryNumber, 
                                    _length, 
                                    _width, 
                                    _height, 
                                    _spaceSpeed, 
                                    _atmosphericSpeed, 
                                    _hyperdriveRating, 
                                    _shields, 
                                    _hull, 
                                    _maneuverability, 
                                    _cargoCapacity, 
                                    _consumables);
        }
    }
}