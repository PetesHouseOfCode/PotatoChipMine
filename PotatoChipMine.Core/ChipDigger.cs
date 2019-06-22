using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core
{
    public class ChipDigger
    {
        private readonly Random _random = new Random();
        private TimeSpan lastDig = TimeSpan.Zero;
        private readonly int secondsBetweenDigs = 15;

        public DiggerClass Class { get; set; }
        public string Name { get; set; }
        public DateTime FirstEquipped { get; set; }
        public int LifeTimeDigs { get; set; } = 0;
        public int LifeTimeChips { get; set; } = 0;
        public int LifeTimeRepairs { get; set; } = 0;

        public int Durability { get; set; }
        public int MaxDurability { get; set; }
        public ChipsHopper Hopper { get; set; }
        public MineSite MineSite { get; set; }
        public List<DiggerEnhancement> Enhancements { get; set; }
        public int LifeTimeBoltsCost { get; set; }
        public int LifeTimeTokensCost { get; set; }

        public DigResult Dig(TimeSpan gameTime)
        {
            LifeTimeDigs++;
            var durabilityHit = RollDurabilityHit();
            Durability -= durabilityHit;
            Durability = Durability < 0 ? 0 : Durability;

            var chips = RollChips();
            Hopper.AddChips(chips);
            LifeTimeChips += chips;
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

        public static ChipDigger StandardDigger(MineSite mineSite)
        {
            return new ChipDigger()
            {
                FirstEquipped = DateTime.Now,
                Class = DiggerClass.Standard,
                MineSite = mineSite,
                Hopper = new ChipsHopper(30),
                Durability = 25,
                MaxDurability = 25,
                Enhancements = new List<DiggerEnhancement>
                {
                    new DiggerEnhancement()
                    {
                        Name = "Hopper +2",
                        Description = "The hopper can be upgraded to level 2 (210 chips)",
                        MaxLevel = 2,
                        CurrentLevel = 0,
                        Slot = DiggerEnhancementSlot.Hopper
                    }
                }
            };
        }

    }


    public class DiggerEnhancement
    {
        public DiggerEnhancementSlot Slot { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxLevel { get; set; }
        public int CurrentLevel { get; set; } = 0;
    }

    public enum DiggerClass
    {
        Standard = 1
    }

    public enum DiggerEnhancementSlot
    {
        Hopper = 1
    }
}