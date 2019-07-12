using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public interface IAchievementReward
    {
        void ApplyReward(GameState gameState);
    }
}