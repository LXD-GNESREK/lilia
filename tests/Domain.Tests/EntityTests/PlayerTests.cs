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
            var org = new Organisation("Test Org");

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
            var org = new Organisation("Test Org");
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
    }
}