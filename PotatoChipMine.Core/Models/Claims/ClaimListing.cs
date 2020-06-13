using PotatoChipMine.Core.Models;
using System;
using System.Linq;
using System.Security.Claims;

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

        public ClaimListing(ClaimListingState state)
        {
            Id = state.Id;
            claim = MineClaim.FromState(state.Claim);
            Price = state.Price;
            LeasePrice = state.LeasePrice;
            Survey = SurveyResults.FromState(state.Survey);
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
        public ClaimListingState GetState()
        {
            return new ClaimListingState
            {
                Id = Id,
                Price = Price,
                LeasePrice = LeasePrice,
                Claim = claim.GetState(),
                Survey = Survey.GetState()
            };
        }
        public static ClaimListing FromState(ClaimListingState state)
        {
            return new ClaimListing(state);
        }
    }
}
