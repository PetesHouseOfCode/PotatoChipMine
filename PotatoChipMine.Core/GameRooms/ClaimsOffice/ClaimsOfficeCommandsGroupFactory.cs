using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Models.Claims;
using System;
using System.Collections.Generic;
using System.Text;

namespace PotatoChipMine.Core.GameRooms.ClaimsOffice
{
    public class ClaimsOfficeCommandsGroupFactory
    {
        readonly ClaimListings listings;
        public ClaimsOfficeCommandsGroupFactory(ClaimListings listings)
        {
            this.listings = listings;
        }

        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup
            {
                LocalCommands = new List<CommandsDefinition>()
                {
                    new CommandsDefinition
                    {
                        CommandText = "buy",
                        EntryDescription = "buy [quantity] [item name] || buy [item name] (to buy single item)",
                        Description = "Purchases the quantity indicated of the item requested.",
                        Command = (userCommand, gameState) => {

                            var command = new BuyClaimCommand { GameState = gameState, Listings = gameState.ClaimsOffice.Listings};
                            if(userCommand.Parameters.Count == 0)
                            {
                                return new FailedMessageCommand("Listing Id is required!");
                            }
                            
                            if(!int.TryParse(userCommand.Parameters[0], out int listingId))
                            {
                                return new FailedMessageCommand("Listing Id is not a number!");
                            }

                            command.ListingId = listingId;                            
                            return command;
                        }
                    },
                    new CommandsDefinition
                    {
                        CommandText = "listings",
                        EntryDescription = "list out available claims",
                        Description = "",
                        Command = (userCommand, gameState) => {

                            var command = new ViewClaimListingsCommand { Listings = gameState.ClaimsOffice.Listings};
                            return command;
                        }
                    }
                }
            };

            return commandsGroup;
        }
    }
}
