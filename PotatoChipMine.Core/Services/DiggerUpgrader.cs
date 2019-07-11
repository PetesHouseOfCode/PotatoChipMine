using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Services
{
    public class DiggerUpgrader
    {
        private static readonly List<UpgradeItem> Upgrades = new List<UpgradeItem>
        {
            new ChipsHopperUpgradeItem
            {
                Name = "Large_Hopper",
                Slot = DiggerUpgradeSlot.Hopper,
                RequiredSlotLevel = 0,
                Level = 1,
                Size = 100
            }
        };

        public static (bool passed, string message) TestUpgradeItem(ChipDigger digger, string upgradeItemName)
        {
            var item = Upgrades.FirstOrDefault(x => x.Name == upgradeItemName);
            if (item == null) return (false, "Item could not be found");

            switch (item.Slot)
            {
                case DiggerUpgradeSlot.Hopper:
                    return TestHopperUpgrade(digger, (ChipsHopperUpgradeItem)item);
            }

            return (false, "Unknown Upgrade.");
        }

        static (bool passed, string message) TestHopperUpgrade(ChipDigger digger, ChipsHopperUpgradeItem item)
        {
            var slot = digger.Upgrades.FirstOrDefault(x => x.Slot == item.Slot);
            if (slot == null) return (false, "This digger doesn't have that upgrade slot available.");

            if (item.RequiredSlotLevel > digger.Hopper.Level)
                return (false,
                    $"You can not perform this upgrade.  You must first level this slot to {item.RequiredSlotLevel}");

            if (item.RequiredSlotLevel + slot.CurrentLevel > slot.MaxLevel)
                return (false,
                    $"You can not perform this upgrade.  The maximum slot level for this digger is {slot.MaxLevel}");

            if (item.Size < digger.Hopper.Max)
                return (false,
                    $"You can not apply this upgrade. This digger can not use it.");

            return (true, "Upgrade is available");
        }

        public static (bool completed, string message) ApplyUpgrade(ChipDigger digger, string upgradeItemName)
        {
            var test = TestUpgradeItem(digger, upgradeItemName);
            if (!test.passed)
                return (test.passed, test.message);

            var item = Upgrades.FirstOrDefault(x => x.Name == upgradeItemName);
            var slot = digger.Upgrades.FirstOrDefault(x => x.Slot == item?.Slot);

            switch (slot.Slot)
            {
                case DiggerUpgradeSlot.Hopper:
                    digger.UpgradeHopper(((ChipsHopperUpgradeItem)item).GetUpgrade());
                    return (true, "Hopper upgraded to Large_Hopper");
            }

            return (true, "Upgrade Applied");
        }
    }

    public class UpgradeItem
    {
        public DiggerUpgradeSlot Slot;
        public string Name { get; set; }
        public int RequiredSlotLevel { get; set; }
    }

    public class ChipsHopperUpgradeItem : UpgradeItem
    {
        public int Size { get; set; }
        public int Level { get; set; }

        public ChipsHopper GetUpgrade()
        {
            return new ChipsHopper(Size, Name, Level);
        }
    }
}