using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Models
{
    public class Miner
    {
        public string Name { get; set; }
        public List<ChipDigger> Diggers { get; set; } = new List<ChipDigger>();
        public int TaterTokens { get; set; }
        public int LifetimeChips { get; set; }
        public int LifetimeTokens { get; set; }
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        public InventoryItem Inventory(string name)
        {
            return InventoryItems.FirstOrDefault(x => x.Name == name);
        }

        public static Miner Default()
        {
            return new Miner
            {
                Diggers = new List<ChipDigger>(),
                TaterTokens = 100,
                InventoryItems = new List<InventoryItem>
                    {new InventoryItem {Name = "chips", Count = 0, InventoryId = 0}}
            };
        }
    }
}