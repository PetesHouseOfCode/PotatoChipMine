using System;
using PotatoChipMine.Models;

namespace PotatoChipMine
{
    public class ChipDigger
    {
        private readonly MineSite _mineSite;
        private readonly Random _random = new Random();

        public ChipDigger(MineSite mineSite)
        {
            _mineSite = mineSite;
        }

        public int Durability { get; set; }

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
            switch (_mineSite.ChipDensity)
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
            switch (_mineSite.Hardness)
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
}