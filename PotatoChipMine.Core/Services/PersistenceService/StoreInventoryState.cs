using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class StoreInventoryState
    {
        public List<StoreItemState> ItemsForSale { get; set; }
        public List<StoreItemState> ItemsBuying { get; set; }
    }
}
