using System;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class InventoryAchievementSetting : AchievementSetting
    {
        public string InventoryItemName { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }

        public static InventoryAchievementSetting DiggerAchievement()
        {
            return new InventoryAchievementSetting
            {
                Id = 1,
                Name = "DiggerAchievement",
                Description = "[Can You Dig It?] --first digger purchase--",
                InventoryItemName = "Digger",
                MinAmount = 1,
                MaxAmount = int.MaxValue
            };
        }
    }
}