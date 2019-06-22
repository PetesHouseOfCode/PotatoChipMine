using System.Collections.Generic;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class LifetimeStatAchievementSetting : AchievementSetting
    {
        public string LifetimeStatName { get; set; }
        public long MinCount { get; set; }


        public static LifetimeStatAchievementSetting ChipsTo50()
        {
            return new LifetimeStatAchievementSetting
            {
                Id = 2,
                Name = "ChipsTo50",
                Description = "YOU REACHED 50 CHIPS!!!",
                LifetimeStatName = Stats.LifetimeChips,
                MinCount = 50,
                Rewards = new List<IAchievementReward>
                {
                    new NewStoreItemReward
                    {
                        ItemId = 0,
                        InventoryId = 3,
                        Name = "Large_Hopper",
                        Description = "Allows digger to hold 50 chips before requiring emptying.",
                        Count = 10,
                        Price = 1200
                    }
                }
            };
        }
    }
}