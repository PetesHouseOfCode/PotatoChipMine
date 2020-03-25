using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class GameAchievement
    {
        protected readonly GameState GameState;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> RewardIds { get; set; } = new List<int>();

        public GameAchievement(GameState gameState)
        {
            GameState = gameState;
        }

        protected virtual bool AchievementReached()
        {
            return GameState.Miner.AttainedAchievements.Any(x => x.Name == Name);
        }

        public virtual AchievementSetting GetSetting()
        {
            return new AchievementSetting
            {
                Id = Id,
                Name = Name,
                Description = Description,
                RewardIds = RewardIds
            };
        }

        public void CheckAchievement()
        {
            if (!AchievementReached())
                return;

            Game.WriteLine($"--Achievement: {Description} has been attained.", PcmColor.Black, PcmColor.Magenta,
                GameConsoles.Events);
            RegisterAchievement();
            foreach (var rewardId in RewardIds)
            {
                var reward = Game.Gateway.Rewards.GetAll().First(x => x.Id == rewardId);
                Game.ApplyReward(reward);
            }
        }

        private void RegisterAchievement()
        {
            GameState.Miner.AttainedAchievements.Add(new PlayerAchievement
            {
                Name = Name,
                Description = Description,
                Achieved = DateTime.Now
            });
        }
    }
}