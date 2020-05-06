using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Models.Claims
{
    public class ClaimLease
    {
        public int Price { get; }

        public MineClaim Claim { get; }

        public ClaimLease(MineClaim claim, int price)
        {
            Claim = claim;
            Price = price;
        }
    }
}
