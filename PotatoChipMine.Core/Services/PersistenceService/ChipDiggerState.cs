using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class ChipDiggerState
    {
        public string Name { get; set; }
        public DateTime FirstEquipped { get; set; }
        public TimeSpan LastDig { get; set; }
        public int Durability { get; set; }
        public int MaxDurability { get; set; }
        public List<DiggerUpgrade> Upgrades { get; set; }
        public ChipsHopperState Hopper { get; set; }
        public MineSiteState MineSite { get; set; }
        public List<Stat> LifeTimeStats { get; set; }
    }
}
