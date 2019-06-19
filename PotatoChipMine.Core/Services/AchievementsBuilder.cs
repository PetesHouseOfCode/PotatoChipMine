using System.Collections.Generic;
using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.Services
{
    public class AchievementsBuilder
    {
        public static List<GameAchievement> BuildAchievementsList(GameState gameState)
        {
            var returnList = new List<GameAchievement> {new DiggerAchievement(gameState)};
            return returnList;
        }

        public static List<PlayerAchievement> GetPotentialAchievements()
        {
            return new List<PlayerAchievement>()
            {
                new PlayerAchievement() {Name = "DiggerAchievement",Description = "[Can You Dig It?] --first digger purchase--"}
            };
        }
    }
}