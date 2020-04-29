using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoChipMine.Core.GameRooms.ClaimsOffice
{
    public class ClaimsOfficeRoomFactory
    {
        private readonly GameState _gameState;
        private readonly CommandsGroup _baseCommandsGroup;
        readonly DataGateway _gateway;

        public ClaimsOfficeRoomFactory(GameState gameState, CommandsGroup baseCommandsGroup, DataGateway gateway)
        {
            _gateway = gateway;
            _gameState = gameState;
            _baseCommandsGroup = baseCommandsGroup;
        }

        public ClaimsOfficeRoom Build()
        {
            var greeting = new[]
            {
                "Welcome to the claims office.",
                "You can type [help] for a list of the store commands.",
                "To leave the office type exit."
            };

            var listings = new ClaimListings();
            var claim = new MineClaim
            {
                ChipDensity = ChipDensity.Normal,
                Hardness = SiteHardness.Hard
            };

            listings.Add(new ClaimListing(claim, 10, 15, SurveyResults.NoSurvey()));
            listings.Add(new ClaimListing(claim, 10, 15, SurveyResults.GetFromClaim(claim)));

            var claimsOffice = new ClaimsOfficeRoom(
                _gameState,
                greeting,
                _baseCommandsGroup.Join(new ClaimsOfficeCommandsGroupFactory(listings).Build()),
                listings
            );

            return claimsOffice;
        }
    }
}
