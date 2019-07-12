using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class MineSiteState
    {
        public ChipDensity ChipDensity { get; set; }
        public SiteHardness Hardness { get; set; }
    }
}
