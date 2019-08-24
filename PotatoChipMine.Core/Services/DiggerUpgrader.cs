using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.DiggerUpgrades;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Services
{
    public class DiggerUpgrader
    {
        static (bool passed, string message) TestHopperUpgrade(ChipDigger digger, ChipsHopperUpgradeItem item)
        {
            var slot = digger.Upgrades.FirstOrDefault(x => x.Slot == DiggerUpgradeSlot.Hopper);
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

        public static (bool completed, string message) ApplyUpgrade(ChipDigger digger, DiggerUpgradeItem item)
        {
            if (item is ChipsHopperUpgradeItem)
            {
                var testresult = TestHopperUpgrade(digger, item as ChipsHopperUpgradeItem);
                if (!testresult.passed)
                    return testresult;
                digger.UpgradeHopper(((ChipsHopperUpgradeItem)item).GetUpgrade());
                return (true, "Hopper upgraded to Large_Hopper");
            }

            if (item is BitUpgradeItem)
            {
                digger.UpgradeBit(((BitUpgradeItem)item).GetUpgrade());
            }

            return (true, "Upgrade Applied");
        }
    }

    public class DiggerUpgradeItem : GameItem
    {
        public int RequiredSlotLevel { get; set; }
        public int Level { get; set; }
    }

    public class ChipsHopperUpgradeItem : DiggerUpgradeItem
    {
        public int Size { get; set; }

        public ChipsHopper GetUpgrade()
        {
            return new ChipsHopper(Size, Name, Level);
        }
    }

    public class BitUpgradeItem : DiggerUpgradeItem
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public ChipDiggerBit GetUpgrade()
        {
            return new ChipDiggerBit(Name, new Range<int>(Min, Max));
        }
    }
}