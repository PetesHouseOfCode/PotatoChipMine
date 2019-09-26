using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class InventoryAchievement : GameAchievement
    {
        public string InventoryItemName { get; private set; }
        public int MinAmount { get; private set; }

        public InventoryAchievement(InventoryAchievementSetting setting, GameState gameState)
            : base(gameState)
        {
            Id = setting.Id;
            Name = setting.Name;
            Description = setting.Description;
            Rewards = setting.Rewards;
            InventoryItemName = setting.InventoryItemName;
            MinAmount = setting.MinAmount;
        }

        public override AchievementSetting GetSetting()
        {
            return new InventoryAchievementSetting
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Rewards = Rewards,
                InventoryItemName = InventoryItemName,
                MinAmount = MinAmount
            };
        }

        protected override bool AchievementReached()
        {
            if (base.AchievementReached())
                return true;

            var inventoryItem = GameState.Miner.InventoryItems
                .FirstOrDefault(x => x.Name.Equals(InventoryItemName, StringComparison.OrdinalIgnoreCase));
            if (inventoryItem == null)
                return false;

            if (inventoryItem.Count >= MinAmount)
                return true;

            return false;
        }        
    }
}