using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class LifetimeStatAchievement : GameAchievement
    {
        public long MinCount { get; set; }
        public string LifetimeStatName { get; set; }

        public LifetimeStatAchievement(LifetimeStatAchievementSetting setting, GameState gameState)
            : base(gameState)
        {
            Id = setting.Id;
            Name = setting.Name;
            Rewards = setting.Rewards;
            Description = setting.Description;
            MinCount = setting.MinCount;
            LifetimeStatName = setting.LifetimeStatName;
        }

        public override AchievementSetting GetSetting()
        {
            return new LifetimeStatAchievementSetting
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Rewards = Rewards,
                MinCount = MinCount,
                LifetimeStatName = LifetimeStatName
            };
        }

        protected override bool AchievementReached()
        {
            if (base.AchievementReached())
                return true;

            var stat = GameState.Miner.GetLifeTimeStat(LifetimeStatName);
            if (stat >= MinCount)
                return true;

            return false;
        }
    }
}