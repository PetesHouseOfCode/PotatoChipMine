using PotatoChipMine.Core.Models.DiggerUpgrades;
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

        public ChipDiggerBit DiggerBit { get; private set; }
        public DiggerDurability Durability { get; private set; }
        public ChipsHopper Hopper { get; private set; }
        //public List<UpgradeSlot> UpgradeSlots { get; set; } = new List<UpgradeSlot>();
        public MineClaim MineClaim { get; private set; }
        public List<DiggerUpgrade> AvailableUpgrades { get; private set; }

        private ChipDigger(ChipDiggerState state)
        {
            Name = state.Name;
            lastDig = state.LastDig;
            FirstEquipped = state.FirstEquipped;
            DiggerBit = ChipDiggerBit.From(state.DiggerBit);
            Durability = DiggerDurability.From(state.Durability);
            MineClaim = new MineClaim(
                state.MineClaim.Id,
                state.MineClaim.ChipDensity,
                state.MineClaim.Hardness
                );
            AvailableUpgrades = state.Upgrades;
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
            Durability.Damage(durabilityHit);

            var chips = RollChips();
            Hopper.AddChips(chips);
            UpdateLifetimeStat(DiggerStats.LifetimeChips, chips);

            return DigResult.Success(chips, durabilityHit);
        }

        private int RollChips()
        {
            switch (MineClaim.ChipDensity)
            {
                case ChipDensity.Scarce:
                    return DiggerBit.Dig(0.2m);
                case ChipDensity.Normal:
                    return DiggerBit.Dig(0.4m);
                case ChipDensity.Rich:
                    return DiggerBit.Dig(1m);
                default:
                    return 0;
            }
        }

        private int RollDurabilityHit()
        {
            switch (MineClaim.Hardness)
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

            if (Durability.NeedsService())
                message.Add($"****** {Name} needs repair! ******");

            return message;
        }

        public void Repair()
        {
            if (Durability.NeedsService() && digFailed)
                lastDig = TimeSpan.Zero;

            Durability.Service();
        }

        public void UpgradeHopper(ChipsHopper hopper)
        {
            Hopper = hopper;
        }
        public void UpgradeBit(ChipDiggerBit bit)
        {
            DiggerBit = bit;
        }

        public int Empty()
        {
            if (Hopper.IsFull && digFailed)
                lastDig = TimeSpan.Zero;

            return Hopper.Empty();
        }

        public static ChipDigger StandardDigger(MineClaim mineClaim)
        {
            return new ChipDigger(
                new ChipDiggerState
                {
                    FirstEquipped = DateTime.Now,
                    MineClaim = new MineClaimState
                    {
                        Id = mineClaim.Id,
                        Hardness = mineClaim.Hardness,
                        ChipDensity = mineClaim.ChipDensity
                    },
                    Hopper = new ChipsHopperState
                    {
                        Max = 30,
                        Name = "Standard"
                    },
                    DiggerBit = new ChipDiggerBitState
                    {
                        Name = "Basic",
                        Min = 8,
                        Max = 25
                    },
                    Durability = new DiggerDurabilityState
                    {
                        Current = 25,
                        Max = 25,
                        Modifier = .2f
                    },
                    Upgrades = new List<DiggerUpgrade>
                    {
                        new DiggerUpgrade()
                        {
                            Name = "Hopper +2",
                            Description = "The hopper can be upgraded to level 2 (210 chips)",
                            MaxLevel = 2,
                            Slot = DiggerUpgradeSlot.Hopper
                        },
                        new DiggerUpgrade()
                        {
                            Name = "Bit upgrade",
                            Description = "The bit can be upgraded to level 2",
                            MaxLevel = 2, 
                            Slot = DiggerUpgradeSlot.Bit
                        }
                    }
                });
        }

        public ChipDiggerState GetState()
        {
            return new ChipDiggerState
            {
                Name = Name,
                LastDig = lastDig,
                FirstEquipped = FirstEquipped,
                DiggerBit = DiggerBit.GetState(),
                Durability = Durability.GetState(),
                Upgrades = AvailableUpgrades,
                Hopper = Hopper.GetState(),
                MineClaim = new MineClaimState
                {
                    ChipDensity = MineClaim.ChipDensity,
                    Hardness = MineClaim.Hardness
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