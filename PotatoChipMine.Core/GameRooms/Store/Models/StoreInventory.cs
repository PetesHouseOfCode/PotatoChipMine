using PotatoChipMine.Core.Services.PersistenceService;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.Store.Models
{
    public class StoreInventory
    {
        public List<StoreItem> ItemsForSale { get; set; } = new List<StoreItem>();
        public List<StoreItem> ItemsBuying { get; set; } = new List<StoreItem>();

        public StoreInventoryState GetState()
        {
            return new StoreInventoryState
            {
                ItemsForSale = ItemsForSale.Select(x =>
                    new StoreItemState
                    {
                        Price = x.Price,
                        Count = x.Count,
                        GameItemState = x.Item.GetState()
                    }).ToList(),
                ItemsBuying = ItemsBuying.Select(x =>
                    new StoreItemState
                    {
                        Price = x.Price,
                        Count = x.Count,
                        GameItemState = x.Item.GetState()
                    }).ToList()
            };
        }

        public static StoreInventory From(StoreInventoryState state)
        {
            return new StoreInventory
            {
                ItemsBuying = state.ItemsBuying.Select(x =>
                    new StoreItem
                    {
                        Price = x.Price,
                        Count = x.Count,
                        Item = GameItemBuilder.Build(x.GameItemState)
                    }).ToList(),
                ItemsForSale = state.ItemsForSale.Select(x =>
                new StoreItem
                {
                    Price = x.Price,
                    Count = x.Count,
                    Item = GameItemBuilder.Build(x.GameItemState)
                }).ToList()
            };
        }
    }
}