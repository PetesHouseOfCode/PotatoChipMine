using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class MinerState
    {
        public string Name { get; set; }
        public List<ChipDiggerState> Diggers { get; set; } = new List<ChipDiggerState>();
        public int TaterTokens { get; set; }
        public List<PlayerAchievement> AttainedAchievements { get; set; } = new List<PlayerAchievement>();
        public List<InventoryItemState> InventoryItems { get; set; } = new List<InventoryItemState>();
        public List<Stat> LifeTimeStats { get; set; } = new List<Stat>();
        public List<ClaimLeaseState> ClaimLeases { get; set; } = new List<ClaimLeaseState>();
    }

    public class ClaimLeaseState
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string DiggerName { get; set; }
        public MineClaimState MineClaim { get; set; }
    }
}
