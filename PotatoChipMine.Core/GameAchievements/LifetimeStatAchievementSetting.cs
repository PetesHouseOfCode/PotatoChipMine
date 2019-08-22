using System.Collections.Generic;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;

namespace PotatoChipMine.Core.GameAchievements
{
    public class LifetimeStatAchievementSetting : AchievementSetting
    {
        public string LifetimeStatName { get; set; }
        public long MinCount { get; set; }


        public static LifetimeStatAchievementSetting ChipsTo500()
        {
            return new LifetimeStatAchievementSetting
            {
                Id = 2,
                Name = "ChipsTo500",
                Description = "YOU REACHED 500 CHIPS!!!",
                LifetimeStatName = Stats.LifetimeChips,
                MinCount = 500,
                Rewards = new List<IAchievementReward>
                {
                    new NewStoreItemReward(
                            count: 10,
                            price: 1200,
                            item: new ChipsHopperUpgradeItem
                            {
                                ItemId = 0,
                                Name = "Large_Hopper",
                                Description = "Allows digger to hold 100 chips before requiring emptying.",
                                RequiredSlotLevel = 0,
                                Level = 1,
                                Size = 100
                            })
                }
            };
        }
    }
}