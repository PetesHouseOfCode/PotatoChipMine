using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class MinerState
    {
        public string Name { get; set; }
        public List<ChipDiggerState> Diggers { get; set; }
        public int TaterTokens { get; set; }
        public List<PlayerAchievement> AttainedAchievements { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public List<Stat> LifeTimeStats { get; set; }
    }
}
