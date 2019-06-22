using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class LifetimeStatAchievement : GameAchievement
    {
        private readonly LifetimeStatAchievementSetting _setting;

        protected override bool AchievementReached()
        {
            if (base.AchievementReached())
                return true;

            var stat = GameState.Miner.GetLifeTimeStat(_setting.LifetimeStatName);
            if (stat >= _setting.MinCount)
                return true;

            return false;
        }

        public LifetimeStatAchievement(LifetimeStatAchievementSetting setting, GameState gameState)
            : base(gameState)
        {
            Name = setting.Name;
            Description = setting.Description;
            Setting = (LifetimeStatAchievementSetting) setting;
            _setting = setting;
        }
    }
}