using System.Linq;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class DiggerAchievement:GameAchievement
    {
        public override bool AchievementReached()
        {
            return (GameState.Miner.InventoryItems.Any(x => x.Name.ToLower() == "digger") &&
                    GameState.Miner.InventoryItems.Count(x => x.Name.ToLower() == "digger") > 0 &&
                    base.AchievementReached());
        }

        public DiggerAchievement(GameState gameState):base(gameState)
        {
            Name = "DiggerAchievement";
            Description = "First digger has been purchased from the store";
        }
    }
}