namespace PotatoChipMine.Models
{
    public class MineSite
    {
        public ChipDensity ChipDensity { get; set; }
        public SiteHardness Hardness { get; set; }
    }

    public enum SiteHardness
    {
        Soft = 0,
        Firm = 1,
        Solid = 2,
        Hard = 3
    }
}