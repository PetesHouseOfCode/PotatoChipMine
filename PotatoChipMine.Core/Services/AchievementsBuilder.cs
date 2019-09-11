using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Services
{
    public class AchievementsBuilder
    {
        public static List<GameAchievement> BuildAchievementsList(GameState gameState)
        {
            var returnList = new List<GameAchievement>
                {
                    new InventoryAchievement(InventoryAchievementSetting.DiggerAchievement(), gameState),
                    new LifetimeStatAchievement(LifetimeStatAchievementSetting.ChipsTo500(), gameState),
                    new LifetimeStatAchievement(LifetimeStatAchievementSetting.LifetimeChipsTo1000(), gameState)
                };

            return returnList;
        }
    }
}