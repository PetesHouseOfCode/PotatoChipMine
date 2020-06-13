using PotatoChipMine.Core;
using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.Claims;

using PotatoChipMineTests.Helpers;
using PotatoChipMineTests.Mocks;

using Shouldly;

using System.Linq;

using Xunit;

namespace PotatoChipMineTests.Commands
{
    public class BuyClaimCommandTests
    {
        private IPotatoChipGame proc;
        private GameState gameState;
        const int MINER_TOKENS = 10;

        public BuyClaimCommandTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            gameState.Miner = Miner.Default();
            gameState.Miner.TaterTokens = MINER_TOKENS;
        }

        [Fact]
        public void Report_error_message_if_listing_id_does_not_exist()
        {
            var command = new BuyClaimCommand();
            command.GameState = gameState;
            command.Listings = new ClaimListings();
            command.Listings.Add(new ClaimListing(new MineClaim(), 1, 2, SurveyResults.NoSurvey()));
            command.ListingId = 2;

            var handler = new BuyClaimCommandHandler();
            handler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"Listing Id 2 is unavailable.");
        }

        [Fact]
        public void Report_error_message_if_miner_does_not_have_enough_tokens()
        {
            var command = new BuyClaimCommand();
            command.GameState = gameState;
            command.Listings = new ClaimListings();
            command.Listings.Add(new ClaimListing(new MineClaim(), MINER_TOKENS + 1, 2, SurveyResults.NoSurvey()));
            command.ListingId = 1;

            var handler = new BuyClaimCommandHandler();
            handler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldStartWith($"You don't have enough tokens.  You need ");
        }

        [Fact]
        public void When_listing_is_purchased_lease_purchase_should_complete()
        {
            var command = new BuyClaimCommand();
            command.GameState = gameState;
            command.Listings = new ClaimListings();
            var claim = new MineClaim();
            var claimPrice = 1;
            command.Listings.Add(new ClaimListing(claim, claimPrice, 2, SurveyResults.NoSurvey()));
            command.ListingId = command.Listings.GetAll().First().Id;

            var handler = new BuyClaimCommandHandler();
            handler.Handle(command);

            command.Listings.GetAll().Count.ShouldBe(0);
            gameState.Miner.TaterTokens.ShouldBe(MINER_TOKENS - claimPrice);
            gameState.Miner.ClaimLeases.GetAll().Count.ShouldBe(1);
            gameState.Miner.ClaimLeases.GetAll().First().Claim.ShouldBe(claim);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldStartWith($"{1} Purchase Complete!");
        }
    }
}
