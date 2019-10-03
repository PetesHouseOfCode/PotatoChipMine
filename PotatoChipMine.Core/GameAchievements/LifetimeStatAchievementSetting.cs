using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System.Collections.Generic;

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
                MinCount = 50,
                Rewards = new List<IAchievementReward>
                {
                    new NewStoreItemReward(
                        id: 1,
                        count: 10,
                        price: 120,
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

        public static LifetimeStatAchievementSetting LifetimeChipsTo1000()
        {
            return new LifetimeStatAchievementSetting
            {
                Id = 3,
                Name = "ChipsTo1000",
                Description = "YOU REACHED 1000 CHIPS!!!!",
                LifetimeStatName = Stats.LifetimeChips,
                MinCount = 100,
                Rewards = new List<IAchievementReward>
                {
                    new NewStoreItemReward
                    (
                        id: 2,
                        count: 10,
                        price: 120,
                        item: new BitUpgradeItem
                        {
                            Name = "Standard_Bit",
                            Min = 12,
                            Max = 40,
                            RequiredSlotLevel = 0,
                            Level = 1,
                        }
                    )
                }
            };
        }
    }
}