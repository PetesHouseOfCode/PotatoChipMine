using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services
{
    public class MineSiteFactory
    {
        private readonly Random _random = new Random();

        public MineClaim BuildSite()
        {
            return new MineClaim
            {
                ChipDensity = (ChipDensity)_random.Next(1, 4),
                Hardness = (SiteHardness)_random.Next(1, 5)
            };
        }
    }
}