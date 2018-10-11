using PotatoChipMine;
using PotatoChipMine.Models;
using Shouldly;
using Xunit;

namespace PotatoChipMineTests
{
    public class ChipDiggerTests
    {
        [Fact]
        public void ChipDigger_Dig_DiggerDurabilityDecreasedBy1()
        {
            var chipDigger = new ChipDigger(new MineSite()) {Durability = 10};
            chipDigger.Dig();
            chipDigger.Durability.ShouldBe(9);
        }

        [Fact]
        public void ChipDigger_Dig_HavingDurabilityEqualsZero_ReturnsEmptyScoop()
        {
            var chipDigger = new ChipDigger(new MineSite()) {Durability = 0};
            var scoop = chipDigger.Dig();
            scoop.Chips.ShouldBe(0);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsRich_ChipsYieldIsBetween7and25()
        {
            var mineSite = new MineSite {ChipDensity = ChipDensity.Rich};
            var chipDigger = new ChipDigger(mineSite) {Durability = 1};
            var scoop = chipDigger.Dig();
            scoop.Chips.ShouldBeGreaterThanOrEqualTo(7);
            scoop.Chips.ShouldBeLessThanOrEqualTo(25);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsNormal_ChipsYieldIsBetween3and7()
        {
            var mineSite = new MineSite { ChipDensity = ChipDensity.Normal };
            var chipDigger = new ChipDigger(mineSite) { Durability = 1 };
            var scoop = chipDigger.Dig();
            scoop.Chips.ShouldBeGreaterThanOrEqualTo(3);
            scoop.Chips.ShouldBeLessThanOrEqualTo(7);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsScarce_ChipsYieldIsBetween0and3()
        {
            var mineSite = new MineSite { ChipDensity = ChipDensity.Normal };
            var chipDigger = new ChipDigger(mineSite) { Durability = 1 };
            var scoop = chipDigger.Dig();
            scoop.Chips.ShouldBeGreaterThanOrEqualTo(3);
            scoop.Chips.ShouldBeLessThanOrEqualTo(7);
        }
    }
}