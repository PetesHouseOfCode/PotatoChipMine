using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class MineClaimState
    {
        public ChipDensity ChipDensity { get; set; }
        public SiteHardness Hardness { get; set; }
    }
}
