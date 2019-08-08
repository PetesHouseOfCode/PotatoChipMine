using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class NewStoreItemReward : StoreItem, IAchievementReward
    {
        public void ApplyReward(GameState gameState)
        {
            gameState.Store.StoreState.ItemsForSale.Add(this);
            Game.WriteLine($"*** A new item is for sale at the store [{Name}]",PcmColor.Green,null,GameConsoles.Events);
            Game.WriteLine(this.Description, PcmColor.Green, null, GameConsoles.Events);
        }
    }
}