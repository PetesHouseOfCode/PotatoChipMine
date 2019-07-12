using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Models
{
    public class ChipDigger : PersistentGameElement
    {
        private readonly Random _random = new Random();
        private TimeSpan lastDig = TimeSpan.Zero;
        private readonly int secondsBetweenDigs = 15;
        private bool digFailed = false;

        public DiggerClass Class { get; private set; } = DiggerClass.Standard;
        public DateTime FirstEquipped { get; private set; }

        public int Durability { get; private set; }
        public int MaxDurability { get; private set; }
        public ChipsHopper Hopper { get; private set; }
        //public List<UpgradeSlot> UpgradeSlots { get; set; } = new List<UpgradeSlot>();
        public MineSite MineSite { get; private set; }
        public List<DiggerUpgrade> Upgrades { get; private set; }

        private ChipDigger(ChipDiggerState state)
        {
            Name = state.Name;
            FirstEquipped = state.FirstEquipped;
            Durability = state.Durability;
            MaxDurability = state.MaxDurability;
            MineSite = new MineSite
                {
                    ChipDensity = state.MineSite.ChipDensity,
                    Hardness = state.MineSite.Hardness
                };
            Upgrades = state.Upgrades;
            LifetimeStats = state.LifeTimeStats ?? new List<Stat>();
            Hopper = ChipsHopper.FromState(state.Hopper);
        }

        public DigResult Dig(TimeSpan gameTime)
        {
            lastDig = gameTime;
            
            var faultMessages = GetFaultMessages();
            if (faultMessages.Any())
            {
                digFailed = true;
                return DigResult.Fault(faultMessages);
            }

            digFailed = false;
            UpdateLifetimeStat(DiggerStats.LifetimeDigs, 1);
            
            var durabilityHit = RollDurabilityHit();
            Durability -= durabilityHit;
            Durability = Durability < 0 ? 0 : Durability;
            
            var chips = RollChips();
            Hopper.AddChips(chips);
            UpdateLifetimeStat(DiggerStats.LifetimeChips, chips);

            return DigResult.Success(chips, durabilityHit);
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

        public bool ReadyToDig(TimeSpan gameTime)
        {
            if (lastDig != TimeSpan.Zero && gameTime.Subtract(lastDig).TotalSeconds < secondsBetweenDigs)
            {
                return false;
            }

            return true;
        }

        private List<string> GetFaultMessages()
        {
            var message = new List<string>();

            if (Hopper.IsFull)
                message.Add($"{Name} --The digger hopper is full.");

            if (IsBroken())
                message.Add($"****** {Name} needs repair! ******");

            return message;
        }

        public bool IsBroken()
        {
            return Durability <= 0;
        }

        public void Repair()
        {
            if(IsBroken() && digFailed)
                lastDig = TimeSpan.Zero;

            Durability = MaxDurability;
        }

        public void UpgradeHopper(ChipsHopper hopper)
        {
            Hopper = hopper;
        }

        public int Empty()
        {
            if(Hopper.IsFull && digFailed)
                lastDig = TimeSpan.Zero;

            return Hopper.Empty();
        }

        public static ChipDigger StandardDigger(MineSite mineSite)
        {
            return new ChipDigger(
                new ChipDiggerState
                {
                    FirstEquipped = DateTime.Now,
                    MineSite = new MineSiteState
                        {
                            Hardness = mineSite.Hardness,
                            ChipDensity = mineSite.ChipDensity
                        },
                    Hopper = new ChipsHopperState
                    {
                        Max = 30,
                        Name = "Standard"
                    },
                    Durability = 25,
                    MaxDurability = 25,
                    Upgrades = new List<DiggerUpgrade>
                    {
                        new DiggerUpgrade()
                        {
                            Name = "Hopper +2",
                            Description = "The hopper can be upgraded to level 2 (210 chips)",
                            MaxLevel = 2,
                            Slot = DiggerUpgradeSlot.Hopper
                        }
                    }
                });
        }

        public ChipDiggerState GetState()
        {
            return new ChipDiggerState
            {
                Name = Name,
                FirstEquipped = FirstEquipped,
                Durability = Durability,
                MaxDurability = MaxDurability,
                Upgrades = Upgrades,
                Hopper = Hopper.GetState(),
                MineSite = new MineSiteState
                {
                    ChipDensity = MineSite.ChipDensity,
                    Hardness = MineSite.Hardness
                },
                LifeTimeStats = LifetimeStats
            };
        }

        public static ChipDigger FromState(ChipDiggerState state)
        {
            return new ChipDigger(state);
        }
    }
}