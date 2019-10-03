using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameAchievements
{
    public class NewStoreItemReward : IAchievementReward
    {
        public GameItem Item { get; private set; }
        public int Count { get; private set; }
        public int Price { get; private set; }
        public int Id { get; private set; }

        public NewStoreItemReward(int id, int count, int price, GameItem item)
        {
            Id = id;
            Price = price;
            Count = count;
            Item = item;
        }
    }
}