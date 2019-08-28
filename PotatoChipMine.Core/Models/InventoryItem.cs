namespace PotatoChipMine.Core.Models
{
    public class InventoryItem
    {
        public string Name => Item.Name;
        public int Count { get; set; }
        public GameItem Item { get; set; }
    }
}