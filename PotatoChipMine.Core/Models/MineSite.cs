namespace PotatoChipMine.Core.Models
{
    public class MineSite
    {
        public ChipDensity ChipDensity { get; set; }
        public SiteHardness Hardness { get; set; }
    }

    public enum SiteHardness
    {
        Soft = 1,
        Firm = 2,
        Hard = 3,
        Solid = 4
    }
}