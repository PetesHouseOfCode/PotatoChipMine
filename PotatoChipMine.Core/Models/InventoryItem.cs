namespace PotatoChipMine.Core.Models
{
    public class InventoryItem
    {
        public string Name => Item.Name;        
        public int Count { get; set; }
        public GameItem Item { get; set; }
    }
    
    public class GameItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}