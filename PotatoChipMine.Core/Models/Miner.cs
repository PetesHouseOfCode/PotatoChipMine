using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Core.Services;
using PotatoChipMine.Core.Services.PersistenceService;

namespace PotatoChipMine.Core.Models
{
    public class Miner:PersistentGameElement
    {
        public List<ChipDigger> Diggers { get; set; } = new List<ChipDigger>();
        public int TaterTokens { get; set; }
        public List<PlayerAchievement> AttainedAchievements { get; set; } = new List<PlayerAchievement>();
        public List<PlayerAchievement> PotentialAchievements { get; set; } = new List<PlayerAchievement>();
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        private Miner(string name, int taterTokens)
        {
            Name = name;
            TaterTokens = taterTokens;
            InventoryItems.Add(new InventoryItem { Name = "chips", Count = 0, InventoryId = 0 });
            PotentialAchievements.AddRange(AchievementsBuilder.GetPotentialAchievements());
        }

        private Miner(MinerState state)
        {
            Name = state.Name;
            TaterTokens = state.TaterTokens;
            InventoryItems = state.InventoryItems;
            PotentialAchievements = state.PotentialAchievements;
            AttainedAchievements = state.AttainedAchievements;
            LifetimeStats = state.LifeTimeStats;
            Diggers.AddRange(state.Diggers.Select(x => ChipDigger.FromState(x)));
        }

        public InventoryItem Inventory(string name)
        {
            return InventoryItems.FirstOrDefault(x => x.Name == name);
        }

        public static Miner Default()
        {
            return new Miner("None", 100);
        }

        public MinerState GetState()
        {
            return new MinerState
            {
                Name = this.Name,
                Diggers = Diggers.Select(x => x.GetState()).ToList(),
                TaterTokens = TaterTokens,
                AttainedAchievements = AttainedAchievements,
                PotentialAchievements = PotentialAchievements,
                InventoryItems = InventoryItems,
                LifeTimeStats = LifetimeStats
            };
        }

        public static Miner FromState(MinerState state)
        {
            return new Miner(state);

        }
    }
}