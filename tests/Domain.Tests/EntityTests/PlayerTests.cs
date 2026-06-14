// tests/Domain.Tests/EntityTests/PlayerTests.cs

using Lilia.Domain.Entities;

namespace Lilia.Domain.Tests.EntityTests
{
    public class PlayerTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            ulong expectedId = 123456789012345678;

            var player = new Player(expectedId);

            Assert.Equal(expectedId, player.DiscordID);
            Assert.NotNull(player.UnitCount);
            Assert.Empty(player.Organisations);
        }

        [Fact]
        public void AddOrganisation_Success()
        {
            var player = new Player(123456789);
            var org = new Organisation("Test Org", "TEST");

            player.AddOrganisation(org);

            Assert.Single(player.Organisations);
            Assert.Contains(org, player.Organisations);
        }

        [Fact]
        public void AddOrganisation_Null_ShouldThrowException()
        {
            var player = new Player(123456789);

            Assert.Throws<ArgumentNullException>(() => player.AddOrganisation(null!));
        }

        [Fact]
        public void RemoveOrganisation_Success()
        {
            var player = new Player(123456789);
            var org = new Organisation("Test Org", "TEST");
            player.AddOrganisation(org);

            player.RemoveOrganisation(org);

            Assert.Empty(player.Organisations);
        }

        [Fact]
        public void RemoveOrganisation_Null_ShouldThrowException()
        {
            var player = new Player(123456789);

            Assert.Throws<ArgumentNullException>(() => player.RemoveOrganisation(null!));
        }

        [Fact]
        public void GetUnassignedUnitCount_ShouldReturnDifferenceBetweenTotalAndAssigned()
        {
            var player = new Player(12345);
            player.UnitCount.AddUnit("TIE/LN", 24); // Player owns 24 total

            var org = new Organisation("Squadron", "SQUAD");
            
            // Assign 4 to the organisation
            org.Add(new Lilia.Domain.Tests.Mocks.TestUnit("TIE/LN", 100, 0, "001"));
            org.Add(new Lilia.Domain.Tests.Mocks.TestUnit("TIE/LN", 100, 0, "002"));
            org.Add(new Lilia.Domain.Tests.Mocks.TestUnit("TIE/LN", 100, 0, "003"));
            org.Add(new Lilia.Domain.Tests.Mocks.TestUnit("TIE/LN", 100, 0, "004"));

            player.AddOrganisation(org);

            int unassigned = player.GetUnassignedUnitCount("TIE/LN");

            Assert.Equal(20, unassigned);
        }
    }
}