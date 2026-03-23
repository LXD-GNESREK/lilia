// tests/Domain.Tests/BaseUnitTests.cs

using Lilia.Domain.Tests.Mocks;

namespace Lilia.Domain.Tests
{
    public class BaseUnitTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            string name = "Test Unit";
            int price = 100;
            int modslots = 2;
            string registryNumber = "12345";

            // Act
            var unit = new TestUnit(name, price, modslots, registryNumber);

            // Assert
            Assert.Equal(name, unit.Name);
            Assert.Equal(price, unit.Price);
            Assert.Equal(modslots, unit.Modslots);
            Assert.Equal(registryNumber, unit.RegistryNumber);
            Assert.NotEqual(Guid.Empty, unit.InternalID);
        }

        [Fact]
        public void Total_ShouldReturnPrice()
        {
            // Arrange
            var unit = new TestUnit("Test Unit", 100, 2, "12345");

            // Act
            int total = unit.Total();

            // Assert
            Assert.Equal(100, total);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ShouldThrowException_WhenNameIsInvalid(string? invalidName)
        {
            // Arrange
            int price = 100;
            int modslots = 2;
            string registryNumber = "12345";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new TestUnit(invalidName!, price, modslots, registryNumber));
        }

        [Fact]
        public void ShouldThrowException_WhenPriceIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TestUnit("Name", -1, 2, "123"));
        }

        [Fact]
        public void ShouldThrowException_WhenModslotsAreNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new TestUnit("Name", 100, -1, "123"));
        }

        [Fact]
        public void DisplayTag_ShouldFormatCorrectly()
        {
            // Arrange
            var unit = new TestUnit("X-Wing", 100, 2, "0042");

            // Act
            string tag = unit.DisplayTag;

            // Assert
            Assert.Equal("X-Wing (TST-0042)", tag);
        }
    }
}