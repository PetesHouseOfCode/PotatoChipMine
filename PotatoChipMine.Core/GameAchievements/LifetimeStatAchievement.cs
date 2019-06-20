using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class LifetimeStatAchievement : GameAchievement
    {
        LifetimeStatAchievementSetting setting;

        protected override bool AchievementReached()
        {
            if (base.AchievementReached())
                return true;

            var stat = GameState.Miner.GetLifeTimeStat(setting.LifetimeStatName);
            if (stat >= setting.MinCount)
                return true;

            return false;
        }

        public LifetimeStatAchievement(LifetimeStatAchievementSetting setting, GameState gameState)
            : base(gameState)
        {
            Name = setting.Name;
            Description = setting.Description;
            this.setting = setting;
        }
    }
}