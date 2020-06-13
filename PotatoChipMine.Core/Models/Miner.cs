using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models.Claims;
using PotatoChipMine.Core.Services;
using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PotatoChipMine.Core.Models
{
    public class Miner : PersistentGameElement
    {
        public List<ChipDigger> Diggers { get; set; } = new List<ChipDigger>();
        public int TaterTokens { get; set; }
        public List<PlayerAchievement> AttainedAchievements { get; set; } = new List<PlayerAchievement>();
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public ClaimLeases ClaimLeases { get; set; } = new ClaimLeases();

        private Miner(string name, int taterTokens)
        {
            Name = name;
            TaterTokens = taterTokens;
            InventoryItems.Add(new InventoryItem
            {
                Count = 0,
                Item = Game.Gateway.GameItems.GetAll().First(gi => gi.Id == 3)
            });
        }

        private Miner(MinerState state)
        {
            Name = state.Name;
            TaterTokens = state.TaterTokens;
            InventoryItems = state.InventoryItems.Select(x =>
                new InventoryItem
                {
                    Count = x.Count,
                    Item = Game.Gateway.GameItems.GetAll().First(gi => gi.Id == x.ItemId)
                }).ToList();
            AttainedAchievements = state.AttainedAchievements;
            LifetimeStats = state.LifeTimeStats;
            Diggers.AddRange(state.Diggers.Select(x => ChipDigger.FromState(x)));
            ClaimLeases = ClaimLeases.FromState(state.ClaimLeases);
        }

        public InventoryItem Inventory(string name)
        {
            return InventoryItems
                .FirstOrDefault(x =>
                    string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase) ||
                    string.Equals(x.Item.PluralizedName, name, StringComparison.CurrentCultureIgnoreCase));
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
                InventoryItems = InventoryItems.Select(x =>
                    new InventoryItemState
                    {
                        Count = x.Count,
                        ItemId = x.Item.Id
                    }).ToList(),
                LifeTimeStats = LifetimeStats,
                ClaimLeases = ClaimLeases.GetState().ToList()
            };
        }

        public static Miner FromState(MinerState state)
        {
            return new Miner(state);
        }
    }
}