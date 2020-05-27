using System;
using System.Collections.Generic;
using PotatoChipMine.Core;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services.PersistenceService;
using Shouldly;
using Xunit;

namespace PotatoChipMineTests
{
    public class ChipDiggerTests
    {
        private ChipDiggerState diggerState;

        public ChipDiggerTests()
        {
            diggerState = new ChipDiggerState
            {
                FirstEquipped = DateTime.Now,
                MineClaim = new MineClaimState
                {
                    Hardness = SiteHardness.Firm,
                    ChipDensity = ChipDensity.Normal
                },
                Hopper = new ChipsHopperState
                {
                    Max = 30,
                    Name = "Standard"
                },
                DiggerBit = new ChipDiggerBitState
                {
                    Name = "Basic",
                    Min = 8,
                    Max = 25
                },
                Durability = new DiggerDurabilityState
                {
                    Current = 25,
                    Max = 25,
                    Modifier = 0.2f
                },
                Upgrades = new List<DiggerUpgrade>
                    {
                        new DiggerUpgrade()
                        {
                            Name = "Hopper +2",
                            Description = "The hopper can be upgraded to level 2 (210 chips)",
                            MaxLevel = 2,
                            Slot = DiggerUpgradeSlot.Hopper
                        }
                    }
            };
        }  

        [Fact]
        public void ChipDigger_Dig_HavingDurabilityEqualsZero_ReturnsEmptyScoop()
        {
            diggerState.Durability.Current = 0;
            var chipDigger = ChipDigger.FromState(diggerState);
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.Failed.ShouldBeTrue();
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsRich_ChipsYieldIsBetween7and25()
        {
            var mineClaim = new MineClaimState {ChipDensity = ChipDensity.Rich};
            diggerState.MineClaim = mineClaim;
            diggerState.Durability.Current = 1;
            var chipDigger = ChipDigger.FromState(diggerState);
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBeGreaterThanOrEqualTo(7);
            scoop.ChipsDug.ShouldBeLessThanOrEqualTo(25);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsNormal_ChipsYieldIsBetween3and9()
        {
            var mineClaim = new MineClaimState { ChipDensity = ChipDensity.Normal };
            diggerState.MineClaim = mineClaim;
            diggerState.Durability.Current = 1;
            var chipDigger = ChipDigger.FromState(diggerState);
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBeGreaterThanOrEqualTo(3);
            scoop.ChipsDug.ShouldBeLessThanOrEqualTo(9);
        }

        [Fact]
        public void ChipDigger_HavingSiteChipDensityIsScarce_ChipsYieldIsBetween0and3()
        {
            var mineClaim = new MineClaimState { ChipDensity = ChipDensity.Scarce };
            diggerState.MineClaim = mineClaim;
            diggerState.Durability.Current = 1;
            var chipDigger = ChipDigger.FromState(diggerState);

            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            scoop.ChipsDug.ShouldBeGreaterThanOrEqualTo(0);
            scoop.ChipsDug.ShouldBeLessThanOrEqualTo(5);
        }

        [Fact]
        public void ChipDigger_HavingSiteHardnessIsSoft_DurabilityDecreasesBetween0and1()
        {
            var mineClaim = new MineClaim {ChipDensity = ChipDensity.Scarce, Hardness = SiteHardness.Soft};
            var chipDigger = ChipDigger.StandardDigger(mineClaim);
            var scoop = chipDigger.Dig(TimeSpan.FromSeconds(20));
            chipDigger.Durability.Current.ShouldBeGreaterThanOrEqualTo(0);
            chipDigger.Durability.Current.ShouldBeLessThanOrEqualTo(25);
        }
    }
}