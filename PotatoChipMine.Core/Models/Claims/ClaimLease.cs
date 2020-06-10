using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Models.Claims
{
    public class ClaimLease
    {
        public int Id { get; private set; }
        public int Price { get; }
        public string DiggerName { get; private set; }

        public bool InUse { get { return !string.IsNullOrEmpty(DiggerName); } }

        public MineClaim Claim { get; }

        public ClaimLease(MineClaim claim, int price)
        {
            Claim = claim;
            Price = price;
        }

        public void SetId(int id)
        {
            this.Id = id;
        }

        public void AssignDigger(ChipDigger digger)
        {
            DiggerName = digger.Name;
        }
    }

    public class ClaimLeases
    {
        private List<ClaimLease> claimLeases = new List<ClaimLease>();

        public IReadOnlyList<ClaimLease> GetAll()
        {
            return claimLeases;
        }

        public void Add(ClaimLease claimLease)
        {
            for (var i = 1; true; i++)
            {
                if (claimLeases.All(x => x.Id != i))
                {
                    claimLease.SetId(i);
                    break;
                }
            }

            claimLeases.Add(claimLease);
        }

        public bool HasId(int claimLeaseId)
        {
            return claimLeases.Any(x => x.Id == claimLeaseId);
        }

        public bool HasClaimsAvailable()
        {
            return claimLeases.Any(x => !x.InUse);
        }
    }
}
