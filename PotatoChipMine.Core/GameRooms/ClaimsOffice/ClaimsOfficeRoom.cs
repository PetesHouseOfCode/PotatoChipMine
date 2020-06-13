using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.Claims;
using PotatoChipMine.Core.Models.DiggerUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public ClaimListings Listings { get; private set; }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.Store;
            base.EnterRoom();
        }

        public void UpdateFromState(List<ClaimListingState> claimListings)
        {
            Listings = ClaimListings.FromState(claimListings);
        }
    }
}
