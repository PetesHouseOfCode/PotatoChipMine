using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameRooms.Store.Models
{
    public class StoreItem
    {
        public string Name => Item.Name;
        public int Price { get; set; }
        public int Count { get; set; }
        public GameItem Item { get; set; }
    }
}