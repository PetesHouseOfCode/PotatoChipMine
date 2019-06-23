namespace PotatoChipMine.Core.Models
{
    public class DiggerUpgrade
    {
        public DiggerUpgradeSlot Slot { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxLevel { get; set; }
        public int CurrentLevel { get; set; } = 0;
    }
}