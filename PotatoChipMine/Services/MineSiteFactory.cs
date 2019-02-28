using System;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class MineSiteFactory
    {
        private readonly Random _random = new Random();

        public MineSite BuildSite()
        {
            return new MineSite
            {
                ChipDensity = (ChipDensity) _random.Next(1, 4),
                Hardness = (SiteHardness) _random.Next(1,5)
            };
        }
    }
}