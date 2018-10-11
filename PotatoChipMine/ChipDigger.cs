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
            Durability--;
            scoop.Chips = RollChips();
            return scoop;
        }

        private int RollChips()
        {
            switch (_mineSite.ChipDensity)
            {
                case ChipDensity.Scarce:
                    return _random.Next(0, 3);
                case ChipDensity.Normal:
                    return _random.Next(3, 7);
                case ChipDensity.Rich:
                    return _random.Next(7, 25);
                default:
                    return 0;
            }
        }
    }
}