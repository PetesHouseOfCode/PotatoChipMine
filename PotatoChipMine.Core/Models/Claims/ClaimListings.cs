using PotatoChipMine.Core.Models.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Models.Claims
{
    public class ClaimListings
    {
        private List<ClaimListing> listings = new List<ClaimListing>();

        public ClaimListings() { }

        private ClaimListings(List<ClaimListingState> state)
        {
            listings.AddRange(state.Select(x => ClaimListing.FromState(x)));
        }

        public void Add(ClaimListing listing)
        {
            for (var i = 1; true; i++)
            {
                if (listings.All(x => x.Id != i))
                {
                    listing.SetId(i);
                    break;
                }
            }

            listings.Add(listing);
        }

        public void Remove(int id)
        {
            listings.Remove(listings.First(x => x.Id == id));
        }

        public IReadOnlyList<ClaimListing> GetAll()
        {
            return listings;
        }

        public bool HasId(int listingId)
        {
            return listings.Any(x => x.Id == listingId);
        }

        public ClaimListing GetById(int listingId)
        {
            return listings.First(x => x.Id == listingId);
        }
        public IEnumerable<ClaimListingState> GetState()
        {
            foreach(var listing in listings)
            {
                yield return listing.GetState();
            }
        }

        public static ClaimListings FromState(List<ClaimListingState> claimListings)
        {
            return new ClaimListings(claimListings);
        }
    }
}
