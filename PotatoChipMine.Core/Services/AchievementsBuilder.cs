using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.Services
{
    public class AchievementsBuilder
    {
        private static List<PlayerAchievement> playerAchievements = new List<PlayerAchievement>();

        public static List<GameAchievement> BuildAchievementsList(GameState gameState)
        {
            var returnList = new List<GameAchievement>
                {
                    new InventoryAchievement(InventoryAchievementSetting.DiggerAchievement(), gameState),
                    new LifetimeStatAchievement(LifetimeStatAchievementSetting.ChipsTo500(), gameState)
                };

            playerAchievements = returnList.Select(x => new PlayerAchievement { Name = x.Name, Description = x.Description }).ToList();
            return returnList;
        }

        public static List<PlayerAchievement> GetPotentialAchievements()
        {
            return playerAchievements;
        }
    }
}