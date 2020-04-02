using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameRooms.Store.Models
{
    public class StoreItem
    {
        public string Name => Item.Name;

        public int Price { get; set; }

        public int Count { get; set; }

        public int MinCount { get; set; }

        public int SellingPrice { get; set; }

        public int BuyingPrice { get; set; }

        public int MaxCount { get; set; }

        public int GameItemId { get; set; }
        public GameItem Item { get; set; }

    }
}