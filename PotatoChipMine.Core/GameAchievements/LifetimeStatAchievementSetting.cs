using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System.Collections.Generic;

namespace PotatoChipMine.Core.GameAchievements
{
    public class LifetimeStatAchievementSetting : AchievementSetting
    {
        public string LifetimeStatName { get; set; }
        public long MinCount { get; set; }
    }
}