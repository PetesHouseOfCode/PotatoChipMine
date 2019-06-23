using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.Services
{
    public class DiggerUpgrader
    {
        private static readonly List<UpgradeItem> Upgrades = new List<UpgradeItem>
        {
            new UpgradeItem
            {
                Name = "Large_Hopper",
                Slot = DiggerUpgradeSlot.Hopper,
                RequiredSlotLevel = 0,
                RaisesSlotBy = 1,
                RaisesSlotTo = 100
            }
        };

        public static (bool passed, string message) TestUpgradeItem(ChipDigger digger, string upgradeItemName)
        {
            var item = Upgrades.FirstOrDefault(x => x.Name == upgradeItemName);
            if (item == null) return (false, "Item could not be found");

            var slot = digger.Upgrades.FirstOrDefault(x => x.Slot == item.Slot);
            if (slot == null) return (false, "This digger doesn't have that upgrade slot available.");

            if (item.RequiredSlotLevel > slot.CurrentLevel)
                return (false,
                    $"You can not perform this upgrade.  You must first level this slot to {item.RequiredSlotLevel}");

            if (item.RaisesSlotBy + slot.CurrentLevel > slot.MaxLevel)
                return (false,
                    $"You can not perform this upgrade.  The maximum slot level for this digger is {slot.MaxLevel}");
            if (item.RaisesSlotTo < digger.Hopper.Max)
                return (false,
                    $"You can not apply this upgrade. This digger can not use it.");

            return (true, "Upgrade is available");

            
        }

        public static (bool completed, string message) ApplyUpgrade(ChipDigger digger, string upgradeItemName)
        {
            var test = TestUpgradeItem(digger, upgradeItemName);
            if (!test.passed) return (test.passed, test.message);

            var item = Upgrades.FirstOrDefault(x => x.Name == upgradeItemName);
            var slot = digger.Upgrades.FirstOrDefault(x => x.Slot == item?.Slot);
            slot.CurrentLevel += item.RaisesSlotBy;
            if (item.RaisesSlotTo > 0)
            {
                switch (slot.Slot)
                {
                    case DiggerUpgradeSlot.Hopper:
                        digger.Hopper = new ChipsHopper(item.RaisesSlotTo);
                        return (true, "Hopper upgraded to Large_Hopper");
                }
            }

            return (true, "Upgrade Applied");
        }
    }

    internal class UpgradeItem
    {
        public DiggerUpgradeSlot Slot;
        public string Name { get; set; }
        public int RequiredSlotLevel { get; set; }
        public int RaisesSlotTo { get; set; }
        public int RaisesSlotBy { get; set; }
    }
}