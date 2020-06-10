using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services
{
    public class MineClaimFactory
    {
        private readonly Random _random = new Random();

        public MineClaim BuildSite()
        {
            return new MineClaim(
                (ChipDensity)_random.Next(1, 4),
                (SiteHardness)_random.Next(1, 5)
                );
        }
    }
}