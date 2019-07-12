using System;
using System.Linq;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class GameAchievement
    {
        protected readonly GameState GameState;

        public string Name { get; set; }
        public string Description { get; set; }
        public AchievementSetting Setting { get; set; }

        public GameAchievement(GameState gameState)
        {
            GameState = gameState;
        }

        protected virtual bool AchievementReached()
        {
            return GameState.Miner.AttainedAchievements.Any(x => x.Name == Name);
        }

        public void CheckAchievement()
        {
            if (!AchievementReached())
                return;

            Game.WriteLine($"--Achievement: {Description} has been attained.", PcmColor.Black, PcmColor.Magenta, GameConsoles.Events);
            RegisterAchievement();
            foreach (var reward in Setting.Rewards)
            {
                reward.ApplyReward(GameState);
            }
           
        }

        private void RegisterAchievement()
        {
            var achievement = GameState.Miner.PotentialAchievements.First(x => x.Name == Name);
            achievement.Achieved = DateTime.Now;

            GameState.Miner.AttainedAchievements.Add(achievement);
            GameState.Miner.PotentialAchievements.Remove(achievement);
        }
    }
}