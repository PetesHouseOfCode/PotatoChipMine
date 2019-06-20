using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class InventoryAchievement : GameAchievement
    {
        InventoryAchievementSetting setting;

        protected override bool AchievementReached()
        {
            if (base.AchievementReached())
                return true;

            var inventoryItem = GameState.Miner.InventoryItems
                .FirstOrDefault(x => x.Name.Equals(setting.InventoryItemName, StringComparison.OrdinalIgnoreCase));
            if (inventoryItem == null)
                return false;

            if (inventoryItem.Count >= setting.MinAmount && inventoryItem.Count <= setting.MaxAmount)
                return true;

            return false;
        }

        public InventoryAchievement(InventoryAchievementSetting setting, GameState gameState)
            : base(gameState)
        {
            Name = setting.Name;
            Description = setting.Description;
            this.setting = setting;
        }
    }
}