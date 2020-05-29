using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class MineClaimState
    {
        public int Id { get; set; }
        public ChipDensity ChipDensity { get; set; }
        public SiteHardness Hardness { get; set; }
    }
}
