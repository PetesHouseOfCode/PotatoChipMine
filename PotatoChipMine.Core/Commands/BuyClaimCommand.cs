using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.ClaimsOffice;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace PotatoChipMine.Core.Commands
{
    public class BuyClaimCommand : ICommand
    {
        public ClaimListings Listings { get; set; }
        public int ListingId { get; set; }
    }

    public class BuyClaimCommandHandler : ICommandHandler<BuyClaimCommand>
    {
        public void Handle(BuyClaimCommand command)
        {
            Game.WriteLine($"Attempting to buy {command.ListingId}");
        }
    }
}
