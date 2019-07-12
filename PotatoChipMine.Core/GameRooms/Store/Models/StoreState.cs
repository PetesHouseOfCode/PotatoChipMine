using System.Collections.Generic;

namespace PotatoChipMine.Core.GameRooms.Store.Models
{
    public class StoreState
    {
        public List<StoreItem> ItemsForSale { get; set; } = new List<StoreItem>();
        public List<StoreItem> ItemsBuying { get; set; } = new List<StoreItem>();
    }
}