using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Core.Services;

namespace PotatoChipMine.Core.Models
{
    public static class Stats
    {
        public const string LifetimeChips = "LifetimeChips";
        public const string LifetimeTokens = "LifetimeTokens";
    }

    public class Stat
    {
        public string Name { get; set; }
        public long Count { get; set; }
    }

    public class Miner
    {
        public string Name { get; set; }
        public List<ChipDigger> Diggers { get; set; } = new List<ChipDigger>();
        public int TaterTokens { get; set; }
        public List<Stat> LifetimeStats { get; set; } = new List<Stat>();
        public List<PlayerAchievement> AttainedAchievements { get; set; } = new List<PlayerAchievement>();
        public List<PlayerAchievement> PotentialAchievements { get; set; } = new List<PlayerAchievement>();
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        public InventoryItem Inventory(string name)
        {
            return InventoryItems.FirstOrDefault(x => x.Name == name);
        }

        public void UpdateLifetimeStat(string name, long changeBy)
        {
            var stat = LifetimeStats.FirstOrDefault(x => x.Name == name);
            if (stat == null)
            {
                LifetimeStats.Add(new Stat { Name = name, Count = changeBy });
                return;
            }

            stat.Count += changeBy;
        }

        public static Miner Default()
        {
            return new Miner
            {
                Diggers = new List<ChipDigger>(),
                TaterTokens = 100,
                InventoryItems = new List<InventoryItem>
                    {new InventoryItem {Name = "chips", Count = 0, InventoryId = 0}},
                PotentialAchievements = AchievementsBuilder.GetPotentialAchievements()
            };
        }

        internal long GetLifeTimeStat(string name)
        {
            var stat = LifetimeStats.FirstOrDefault(x => x.Name == name);
            if (stat == null)
                return 0;

            return stat.Count;
        }
    }
}