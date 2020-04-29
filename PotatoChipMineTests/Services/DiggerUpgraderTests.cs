using System;
using System.Collections.Generic;
using System.Text;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using Shouldly;
using Xunit;

namespace PotatoChipMineTests.Services
{
    [Collection("Game Test")]
    public class DiggerUpgraderTests
    {
        [Fact]
        public void With_digger_bit_slot_available_allow_bit_upgrade()
        {
            var digger = ChipDigger.StandardDigger(new MineClaim());
            var item = new BitUpgradeItem()
            {
                Name = "better bit",
                Description = "",
                Id = 10,
                Level = 1,
                RequiredSlotLevel = 0,
            };
            var result = DiggerUpgrader.ApplyUpgrade(digger, item);
            result.completed.ShouldBeTrue();
        }

        [Fact]
        public void With_digger_bit_slot_available_but_too_low_level_fails_upgrade()
        {
            var digger = ChipDigger.StandardDigger(new MineClaim());
            var item = new BitUpgradeItem()
            {
                Name = "better bit",
                Description = "",
                Id = 10,
                Level = 2,
                RequiredSlotLevel = 1,
            };
            var result = DiggerUpgrader.ApplyUpgrade(digger, item);
            result.completed.ShouldBeFalse();
        }

        [Fact]
        public void With_digger_bit_slot_available_but_upgrade_to_large_fails_upgrade()
        {
            var digger = ChipDigger.StandardDigger(new MineClaim());
            var item = new BitUpgradeItem()
            {
                Name = "better bit",
                Description = "",
                Id = 10,
                Level = 3,
                RequiredSlotLevel = 0,
            };
            var result = DiggerUpgrader.ApplyUpgrade(digger, item);
            result.completed.ShouldBeFalse();
        }
    }
}
