// tests/Domain.Tests/BuilderTests/StarfighterBuilderTests.cs

using Lilia.Domain.Builders;
using Lilia.Domain.Enums;

namespace Lilia.Domain.Tests.BuilderTests
{
    public class StarfighterBuilderTests
    {
        [Fact]
        public void Build_ShouldCreateStarfighterWithCorrectProperties()
        {
            // Arrange
            var builder = new StarfighterBuilder("X-Wing", 150000, 3, "T-65")
                .WithShields(RelativeRating.AboveAverage)
                .WithHull(RelativeRating.Average)
                .WithManeuverability(RelativeRating.Strong)
                .WithDimensions(12.5, 11.0, 2.4)
                .WithSpaceSpeed(1050)
                .WithAtmosphericSpeed(1200)
                .WithHyperdriveRating(1.0)
                .WithCargoCapacity(110)
                .WithConsumables("1 week");

            // Act
            var starfighter = builder.Build();

            // Assert
            Assert.Equal("X-Wing", starfighter.Name);
            Assert.Equal(150000, starfighter.Price);
            Assert.Equal(3, starfighter.Modslots);
            Assert.Equal("T-65", starfighter.RegistryNumber);
            Assert.Equal(RelativeRating.AboveAverage, starfighter.Shields);
            Assert.Equal(RelativeRating.Average, starfighter.Hull);
            Assert.Equal(RelativeRating.Strong, starfighter.Maneuverability);
            Assert.Equal(12.5, starfighter.Length);
            Assert.Equal(11.0, starfighter.Width);
            Assert.Equal(2.4, starfighter.Height);
            Assert.Equal(1050, starfighter.SpaceSpeed);
            Assert.Equal(1200, starfighter.AtmosphericSpeed);
            Assert.Equal(1.0, starfighter.HyperdriveRating);
            Assert.Equal(110, starfighter.CargoCapacity);
            Assert.Equal("1 week", starfighter.Consumables);
        }
    
        [Fact]
        public void Build_ShouldThrowException_WhenDimensionsAreNegative()
        {
            // Arrange
            var builder = new StarfighterBuilder("X-Wing", 150000, 3, "T-65")
                .WithDimensions(-1, -11, -4);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.Build());
        }

        [Fact]
        public void Build_ShouldThrowException_WhenSpaceSpeedIsNegative()
        {
            // Arrange
            var builder = new StarfighterBuilder("X-Wing", 150000, 3, "T-65")
                .WithSpaceSpeed(-100);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.Build());
        }

        [Fact]
        public void Build_ShouldThrowException_WhenAtmosphericSpeedIsNegative()
        {
            // Arrange
            var builder = new StarfighterBuilder("X-Wing", 150000, 3, "T-65")
                .WithAtmosphericSpeed(-100);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.Build());
        }

        [Fact]
        public void Build_ShouldThrowException_WhenCargoCapacityIsNegative()
        {
            // Arrange
            var builder = new StarfighterBuilder("X-Wing", 150000, 3, "T-65")
                .WithCargoCapacity(-100);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.Build());
        }
    }
}