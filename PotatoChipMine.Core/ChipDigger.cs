using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core
{
    public class ChipDigger
    {
        private readonly Random _random = new Random();
        private TimeSpan lastDig = TimeSpan.Zero;
        private readonly int secondsBetweenDigs = 15;

        public ChipDigger(MineSite mineSite)
        {
            MineSite = mineSite;
            Hopper = new ChipsHopper(30);
            Durability = 25;
            MaxDurability = 25;
        }

        public string Name { get; set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }
        public ChipsHopper Hopper { get; set; }
        public MineSite MineSite { get; set; }

        public DigResult Dig(TimeSpan gameTime)
        {
            var durabilityHit = RollDurabilityHit();
            Durability -= durabilityHit;
            Durability = Durability < 0 ? 0 : Durability;
            
            var chips = RollChips();
            Hopper.AddChips(chips);
            lastDig = gameTime;
            return new DigResult(chips, durabilityHit);
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
        

        public bool CanDig(TimeSpan gameTime)
        {
            if (lastDig != TimeSpan.Zero && gameTime.Subtract(lastDig).TotalSeconds < secondsBetweenDigs)
            {
                return false;
            }
            
            return !Hopper.IsFull && Durability > 0;
        }

    }
}