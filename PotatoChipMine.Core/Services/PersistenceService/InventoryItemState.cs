using System;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class InventoryItemState
    {
        public int Count { get; set; }
        public GameItemState GameItemState { get; set; }
    }
}
