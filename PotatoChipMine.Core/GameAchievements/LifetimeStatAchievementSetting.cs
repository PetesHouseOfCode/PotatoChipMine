using PotatoChipMine.Core.Models;
using System;
using System.Linq;

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
                MinCount = 50
            };
        }
    }
}