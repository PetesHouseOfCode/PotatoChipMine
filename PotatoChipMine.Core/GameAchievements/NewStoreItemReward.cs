using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class NewStoreItemReward : IAchievementReward
    {
        readonly GameItem item;
        readonly int count;
        readonly int price;

        public NewStoreItemReward(int count, int price, GameItem item)
        {
            this.price = price;
            this.count = count;
            this.item = item;
        }
        
        public void ApplyReward(GameState gameState)
        {
            gameState.Store.StoreState.ItemsForSale.Add(new StoreItem { Price = price, Count = count, Item = item });
            Game.WriteLine($"*** A new item is for sale at the store [{item.Name}]",PcmColor.Green,null,GameConsoles.Events);
            Game.WriteLine(item.Description, PcmColor.Green, null, GameConsoles.Events);
        }
    }
}