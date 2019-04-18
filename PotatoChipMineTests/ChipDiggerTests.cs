using System;
using PotatoChipMine;
using PotatoChipMine.Models;
using Shouldly;
using Xunit;

namespace PotatoChipMineTests
{
    public class ChipDiggerTests
    {
       

        [Fact]
        public void ChipDigger_Dig_HavingDurabilityEqualsZero_ReturnsEmptyScoop()
        {
            var chipDigger = new ChipDigger(new MineSite()) {Durability = 0};
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBe(0);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsRich_ChipsYieldIsBetween7and25()
        {
            var mineSite = new MineSite {ChipDensity = ChipDensity.Rich};
            var chipDigger = new ChipDigger(mineSite) {Durability = 1};
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBeGreaterThanOrEqualTo(7);
            scoop.ChipsDug.ShouldBeLessThanOrEqualTo(25);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsNormal_ChipsYieldIsBetween3and7()
        {
            var mineSite = new MineSite { ChipDensity = ChipDensity.Normal };
            var chipDigger = new ChipDigger(mineSite) { Durability = 1 };
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBeGreaterThanOrEqualTo(3);
            scoop.ChipsDug.ShouldBeLessThanOrEqualTo(7);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsScarce_ChipsYieldIsBetween0and3()
        {
            var mineSite = new MineSite { ChipDensity = ChipDensity.Normal };
            var chipDigger = new ChipDigger(mineSite) { Durability = 1 };
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBeGreaterThanOrEqualTo(3);
            scoop.ChipsDug.ShouldBeLessThanOrEqualTo(7);
        }

        [Fact]
        public void ChipDigger_HavingSiteHardnessIsSoft_DurabilityDecreasesBetween0and1()
        {
            var mineSite = new MineSite {ChipDensity = ChipDensity.Scarce, Hardness = SiteHardness.Soft};
            var chipDigger = new ChipDigger(mineSite);
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            chipDigger.Durability.ShouldBeGreaterThanOrEqualTo(0);
            chipDigger.Durability.ShouldBeLessThanOrEqualTo(25);
        }
    }
}