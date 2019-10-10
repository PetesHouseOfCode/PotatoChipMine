using System;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class InventoryAchievementSetting : AchievementSetting
    {
        public string InventoryItemName { get; set; }
        public int MinAmount { get; set; }
    }
}