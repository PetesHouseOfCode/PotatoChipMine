using System;
using System.Linq;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class GameAchievement
    {
        protected readonly GameState GameState;

        public string Name { get; set; }
        public string Description { get; set; }

        public GameAchievement(GameState gameState)
        {
            GameState = gameState;
        }

        public virtual bool AchievementReached()
        {
            return GameState.Miner.AttainedAchievements.All(x => x.Name != Name);
        }
        public virtual void RegisterAchievement()
        {
            GameState.Miner.AttainedAchievements.Add(new PlayerAchievement
                { Achieved = DateTime.Now, Description =Description, Name = Name });
        }
    }
}