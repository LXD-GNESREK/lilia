// tests/Domain.Tests/EntityTests/OrganisationTests.cs

using Lilia.Domain.Entities;
using Lilia.Domain.Tests.Mocks;

namespace Lilia.Domain.Tests.EntityTests
{
    public class OrganisationTests
    {
        [Fact]
        public void Organisation_Creation_Success()
        {
            // Arrange
            string name = "Test Organisation";

            // Act
            var organisation = new Organisation(name);

            // Assert
            Assert.NotNull(organisation);
            Assert.NotEqual(Guid.Empty, organisation.InternalID);
            Assert.Equal(name, organisation.Name);
            Assert.Empty(organisation.Elements);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Organisation_Creation_ShouldThrowException_WhenNameIsInvalid(string? invalidName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Organisation(invalidName!));
        }

        [Fact]
        public void Organisation_AddElement_Success()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");
            var element = new TestUnit("Test Unit", 100, 2, "REG123");

            // Act
            organisation.Add(element);

            // Assert
            Assert.Single(organisation.Elements);
            Assert.Contains(element, organisation.Elements);
        }

        [Fact]
        public void Organisation_RemoveElement_Success()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");
            var element = new TestUnit("Test Unit", 100, 2, "REG123");
            organisation.Add(element);

            // Act
            organisation.Remove(element);

            // Assert
            Assert.Empty(organisation.Elements);
        }

        [Fact]
        public void Organisation_AddNull_ShouldThrowException()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => organisation.Add(null!));
        }

        [Fact]
        public void Organisation_AddSelf_ShouldThrowException()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => organisation.Add(organisation));
        }

        [Fact]
        public void Organisation_Total_ShouldReturnZero_WhenNoElements()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");

            // Act
            int total = organisation.Total();

            // Assert
            Assert.Equal(0, total);
        }

        [Fact]
        public void Organisation_Total_ShouldReturnSumOfElements()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");
            var element1 = new TestUnit("Test Unit 1", 100, 2, "REG123");
            var element2 = new TestUnit("Test Unit 2", 200, 3, "REG456");
            organisation.Add(element1);
            organisation.Add(element2);

            // Act
            int total = organisation.Total();

            // Assert
            Assert.Equal(300, total);
        }

        [Fact]
        public void Organisation_Total_ShouldIncludeNestedElements()
        {
            // Arrange
            var organisation = new Organisation("Test Organisation");
            var element1 = new TestUnit("Test Unit 1", 100, 2, "REG123");
            var subOrganisation = new Organisation("Sub Organisation");
            var element2 = new TestUnit("Test Unit 2", 200, 3, "REG456");
            subOrganisation.Add(element2);
            organisation.Add(element1);
            organisation.Add(subOrganisation);

            // Act
            int total = organisation.Total();

            // Assert
            Assert.Equal(300, total);
        }
    }
}