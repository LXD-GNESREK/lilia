// tests/Domain.Tests/EntityTests/UnitCountTests.cs

using Lilia.Domain.Entities;

namespace Lilia.Domain.Tests.EntityTests
{
    public class UnitCountTests
    {
        [Fact]
        public void AddUnit_NewUnit_ShouldSetCount()
        {
            var unitCount = new UnitCount();
            unitCount.AddUnit("TIE/LN", 12);

            Assert.True(unitCount.Units.ContainsKey("TIE/LN"));
            Assert.Equal(12, unitCount.Units["TIE/LN"]);
        }

        [Fact]
        public void AddUnit_ExistingUnit_ShouldIncreaseCount()
        {
            var unitCount = new UnitCount();
            unitCount.AddUnit("TIE/LN", 12);
            unitCount.AddUnit("TIE/LN", 5);

            Assert.Equal(17, unitCount.Units["TIE/LN"]);
        }

        [Fact]
        public void AddUnit_NegativeCount_ShouldDecrease()
        {
            var unitCount = new UnitCount();
            unitCount.AddUnit("TIE/LN", 10);
            unitCount.AddUnit("TIE/LN", -4);

            Assert.Equal(6, unitCount.Units["TIE/LN"]);
        }

        [Fact]
        public void AddUnit_ReduceBelowZero_ShouldThrowException()
        {
            var unitCount = new UnitCount();
            unitCount.AddUnit("TIE/LN", 5);

            Assert.Throws<InvalidOperationException>(() => unitCount.AddUnit("TIE/LN", -10));
        }
    }
}