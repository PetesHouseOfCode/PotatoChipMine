using PotatoChipMine.Core.Services.PersistenceService;

namespace PotatoChipMine.Core.Models
{
    public class MineClaim
    {
        public ChipDensity ChipDensity { get; set; }
        public SiteHardness Hardness { get; set; }

        public MineClaimState GetState()
        {
            return new MineClaimState
            {
                ChipDensity = ChipDensity,
                Hardness = Hardness
            };
        }
    }

    public enum SiteHardness
    {
        Soft = 1,
        Firm = 2,
        Hard = 3,
        Solid = 4
    }
}