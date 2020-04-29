using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.DiggerUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PotatoChipMine.Core.GameRooms.ClaimsOffice
{
    public class ClaimsOfficeRoom : GameRoom
    {
        public ClaimsOfficeRoom(
            GameState gameState,
            string[] greeting,
            CommandsGroup commandsGroup,
            ClaimListings listings)
            : base(gameState, greeting, GameMode.Store)
        {
            CommandsGroup = commandsGroup;
            Listings = listings;
            this.Name = "Claims Office";
        }

        public ClaimListings Listings { get; }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.Store;
            base.EnterRoom();
        }
    }

    public class ClaimListings
    {
        private List<ClaimListing> listings = new List<ClaimListing>();

        public void Add(ClaimListing listing)
        {
            for(var i = 1; true; i++)
            {
                if(listings.All(x=>x.Id != i))
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
    }

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
            if(Id > 0)
            {
                throw new InvalidOperationException("Claim Listing Id is already set.");
            }

            Id = id;
        }
    }

    public class SurveyResults
    {
        public string Density { get; }
        public string Hardness { get; }

        public SurveyResults(string density, string hardness)
        {
            Density = density;
            Hardness = hardness;
        }

        public static SurveyResults NoSurvey()
        {
            return new SurveyResults("Unknown", "Unknown");
        }

        public static SurveyResults GetFromClaim(MineClaim claim)
        {
            return new SurveyResults(claim.ChipDensity.ToString(), claim.Hardness.ToString());
        }
    }
}
