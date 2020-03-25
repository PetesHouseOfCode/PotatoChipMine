using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.DiggerUpgrades;
using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Services
{
    public static class DiggerUpgrader
    {
        static (bool passed, string message) TestHopperUpgrade(ChipDigger digger, ChipsHopperUpgradeItem item)
        {
            var slot = digger.AvailableUpgrades.FirstOrDefault(x => x.Slot == DiggerUpgradeSlot.Hopper);
            if (slot == null) return (false, "This digger doesn't have that upgrade slot available.");

            if (item.RequiredSlotLevel > digger.Hopper.Level)
                return (false,
                    $"You can not perform this upgrade.  You must first level this slot to {item.RequiredSlotLevel}");

            if (item.Level > slot.MaxLevel)
                return (false,
                    $"You can not perform this upgrade.  The maximum slot level for this digger is {slot.MaxLevel}");

            if (item.Size < digger.Hopper.Max)
                return (false,
                    $"You can not apply this upgrade. This digger can not use it.");

            return (true, "Upgrade is available");
        }

        private static (bool passed, string message) TestBitUpgrade(ChipDigger digger, BitUpgradeItem item)
        {
            var slot = digger.AvailableUpgrades.FirstOrDefault(x => x.Slot == DiggerUpgradeSlot.Bit);
            if (slot == null) return (false, "This digger doesn't have that upgrade slot available.");

            if (item.RequiredSlotLevel > digger.DiggerBit.Level)
                return (false,
                    $"You can not perform this upgrade.  You must first level this slot to {item.RequiredSlotLevel}");

            if (item.Level > slot.MaxLevel)
                return (false,
                    $"You can not perform this upgrade.  The maximum slot level for this digger is {slot.MaxLevel}");

            return (true, "Upgrade is available");
        }

        public static (bool completed, string message) ApplyUpgrade(ChipDigger digger, DiggerUpgradeItem item)
        {
            if (item is ChipsHopperUpgradeItem)
            {
                var testResult = TestHopperUpgrade(digger, item as ChipsHopperUpgradeItem);
                if (!testResult.passed)
                    return testResult;
                
                digger.UpgradeHopper(((ChipsHopperUpgradeItem)item).GetUpgrade());
                return (true, "Hopper upgraded to Large_Hopper");
            }

            if (item is BitUpgradeItem)
            {
                var testResult = TestBitUpgrade(digger, item as BitUpgradeItem);
                if (!testResult.passed)
                    return testResult;
                
                digger.UpgradeBit(((BitUpgradeItem)item).GetUpgrade());
            }

            return (true, "Upgrade Applied");
        }
    }

    public abstract class DiggerUpgradeItem : GameItem
    {
        public int RequiredSlotLevel { get; set; }
        public int Level { get; set; }

        public override GameItemState GetState()
        {
            var baseState = base.GetState();
            baseState.MetaData.Add("RequiredSlotLevel", RequiredSlotLevel.ToString());
            baseState.MetaData.Add("Level", Level.ToString());
            return baseState;
        }
    }

    public class ChipsHopperUpgradeItem : DiggerUpgradeItem
    {
        public int Size { get; set; }

        public ChipsHopper GetUpgrade()
        {
            return new ChipsHopper(Size, Name, Level);
        }

        public override GameItemState GetState()
        {
            var baseState = base.GetState();
            baseState.Type = GameItemStateTypes.ChipsHopperUpgradeItem;
            baseState.Type = "ChipsHopperUpgradeItem";
            baseState.MetaData.Add("Size", Size.ToString());
            return baseState;
        }

        public new static GameItem From(GameItemState state)
        {
            return new ChipsHopperUpgradeItem
            {
                Id = state.ItemId,
                Name = state.Name,
                Description = state.Description,
                RequiredSlotLevel = int.Parse(state.MetaData["RequiredSlotLevel"]),
                Level = int.Parse(state.MetaData["Level"]),
                Size = int.Parse(state.MetaData["Size"])
            };
        }
    }

    public class BitUpgradeItem : DiggerUpgradeItem
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public ChipDiggerBit GetUpgrade()
        {
            return new ChipDiggerBit(Name, new Range<int>(Min, Max), Level);
        }

        public override GameItemState GetState()
        {
            var baseState = base.GetState();
            baseState.Type = GameItemStateTypes.BitUpgradeItem;
            baseState.MetaData.Add("Min", Min.ToString());
            baseState.MetaData.Add("Max", Max.ToString());
            return baseState;
        }

        public new static GameItem From(GameItemState state)
        {
            return new BitUpgradeItem
            {
                Id = state.ItemId,
                Name = state.Name,
                Description = state.Description,
                RequiredSlotLevel = int.Parse(state.MetaData["RequiredSlotLevel"]),
                Level = int.Parse(state.MetaData["Level"]),
                Min = int.Parse(state.MetaData["Min"]),
                Max= int.Parse(state.MetaData["Max"])
            };
        }
    }
}