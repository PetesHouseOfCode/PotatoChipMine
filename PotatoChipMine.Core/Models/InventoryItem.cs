namespace PotatoChipMine.Core.Models
{
    public class InventoryItem
    {
        public int ItemId { get; set; }
        public int InventoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
    }
}