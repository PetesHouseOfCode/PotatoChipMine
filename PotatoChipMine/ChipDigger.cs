using System;
using PotatoChipMine.Models;

namespace PotatoChipMine
{
    public class ChipDigger
    {
        private readonly Random _random = new Random();

        public ChipDigger(MineSite mineSite)
        {
            this.MineSite = mineSite;
            this.Hopper = new ChipsHopper {Count = 0, Max = 30};
            this.Durability = 25;
        }

        public string Name { get; set; }
        public int Durability { get; set; }
        public ChipsHopper Hopper { get; set; }
        public MineSite MineSite { get; set; }
        public Scoop Dig()
        {
            var scoop = new Scoop();
            if (Durability < 1) return scoop;
            Durability -= RollDurabilityHit();
            Durability = Durability < 0 ? 0 : Durability;
            scoop.Chips = RollChips();
            return scoop;
        }

        private int RollChips()
        {
            switch (MineSite.ChipDensity)
            {
                case ChipDensity.Scarce:
                    return _random.Next(0, 4);
                case ChipDensity.Normal:
                    return _random.Next(3, 8);
                case ChipDensity.Rich:
                    return _random.Next(8, 25);
                default:
                    return 0;
            }
        }

        private int RollDurabilityHit()
        {
            switch (MineSite.Hardness)
            {
                case SiteHardness.Soft:
                    return _random.Next(0, 3);
                case SiteHardness.Firm:
                    return _random.Next(1, 4);
                case SiteHardness.Hard:
                    return _random.Next(4, 8);
                case SiteHardness.Solid:
                    return _random.Next(8, 13);
                default:
                    return -1;
            }
        }
    }

    public class ChipsHopper
    {
        public int Max { get; set; } = 0;
        public int Count { get; set; } = 0;
        public bool IsFull => Count >= Max;
    }
}