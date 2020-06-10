using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.Claims;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace PotatoChipMine.Core.Commands
{
    public class BuyClaimCommand : CommandWithGameState, ICommand
    {
        public ClaimListings Listings { get; set; }
        public int ListingId { get; set; }
    }

    public class BuyClaimCommandHandler : ICommandHandler<BuyClaimCommand>
    {
        public void Handle(BuyClaimCommand command)
        {
            if(!command.Listings.HasId(command.ListingId))
            {
                Game.WriteLine($"Listing Id {command.ListingId} is unavailable.");
                return;
            }

            var miner = command.GameState.Miner;
            var listings = command.Listings;
            var listing = listings.GetById(command.ListingId);

            if(miner.TaterTokens <= listing.Price)
            {
                var priceShortage = listing.Price - miner.TaterTokens;
                Game.WriteLine($"You don't have enough tokens.  You need {priceShortage} more tokens.");
                return;
            }

            miner.TaterTokens = miner.TaterTokens - listing.Price;
            miner.ClaimLeases.Add(listing.GetLease());
            command.Listings.Remove(listing.Id);

            Game.WriteLine($"{command.ListingId} Purchase Complete!");
        }
    }
}
