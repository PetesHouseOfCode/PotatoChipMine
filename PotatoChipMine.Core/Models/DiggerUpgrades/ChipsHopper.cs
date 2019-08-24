using PotatoChipMine.Core.Services.PersistenceService;
using System;

namespace PotatoChipMine.Core.Models.DiggerUpgrades
{
    public class ChipsHopper
    {
        public string Name { get; private set; } = "None";
        public int Level { get; private set; }
        public int Max { get; set; } = 0;
        public int Count { get; private set; } = 0;
        public bool IsFull => Count >= Max;

        public ChipsHopper(int max, string name = "Starter", int level = 0)
        {
            Max = max;
            Name = name;
            Level = level;
        }

        private ChipsHopper(int max, string name = "Starter", int level = 0, int count = 0)
            : this(max, name, level)
        {
            Count = count;
        }

        public static ChipsHopper Restore(int max, string name, int level, int count)
        {
            return new ChipsHopper(max, name, level, count);
        }

        public void AddChips(int amount)
        {
            if (Count + amount > Max)
            {
                Count = Max;
                return;
            }

            Count += amount;
        }

        public int Empty()
        {
            var count = Count;
            Count = 0;
            return count;
        }

        public ChipsHopperState GetState()
        {
            return new ChipsHopperState
            {
                Name = Name,
                Level = Level,
                Max = Max,
                Count = Count
            };
        }

        public static ChipsHopper FromState(ChipsHopperState chipsHopper)
        {
            return new ChipsHopper(
                       chipsHopper.Max,
                       string.IsNullOrEmpty(chipsHopper.Name) ? "None" : chipsHopper.Name,
                       chipsHopper.Level,
                       chipsHopper.Count);
        }
    }
}