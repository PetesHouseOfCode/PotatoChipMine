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
        public List<IAchievementReward> Rewards { get; set; } = new List<IAchievementReward>();

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
                Description = Description
            };
        }

        public void CheckAchievement()
        {
            if (!AchievementReached())
                return;

            Game.WriteLine($"--Achievement: {Description} has been attained.", PcmColor.Black, PcmColor.Magenta,
                GameConsoles.Events);
            RegisterAchievement();
            foreach (var reward in Rewards)
            {
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