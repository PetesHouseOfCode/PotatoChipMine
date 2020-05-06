using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Models.Claims
{
    public class ClaimListing
    {
        private MineClaim claim;

        public int Id { get; private set; }
        public int Price { get; }
        public int LeasePrice { get; }

        public SurveyResults Survey { get; }

        public ClaimListing(MineClaim claim, int price, int leasePrice, SurveyResults surveyResults)
        {
            this.claim = claim;
            Price = price;
            LeasePrice = leasePrice;
            Survey = surveyResults;
        }

        public void SetId(int id)
        {
            if (Id > 0)
            {
                throw new InvalidOperationException("Claim Listing Id is already set.");
            }

            Id = id;
        }

        public ClaimLease GetLease()
        {
            return new ClaimLease(claim, LeasePrice);
        }
    }
}
