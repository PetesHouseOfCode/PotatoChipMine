using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.Store.Services
{
    public class StoreCommandsGroupFactory
    {
        private readonly StoreInventory _storeState;
        private readonly GameState _gameState;

        public StoreCommandsGroupFactory(GameState gameState, StoreInventory storeState)
        {
            _gameState = gameState;
            _storeState = storeState;
        }

        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup
            {
                LocalCommands = new List<CommandsDefinition>()
                {
                    new CommandsDefinition()
                    {
                        CommandText = "sell",
                        EntryDescription = "sell [quantity] [item name] || sell [item name] (sells all) ",
                        Description = "Sell all of the indicated items in your inventory.",
                        Command = (userCommand, gameState) => {

                            if(userCommand.Parameters.Count() == 0 || userCommand.Parameters.Count() > 2)
                            {
                                return new FailedMessageCommand($"Invalid Quantity");
                            }

                            if(userCommand.Parameters.Count() == 2)
                            {
                                if(!int.TryParse(userCommand.Parameters[0], out var quantity))
                                {
                                    return new FailedMessageCommand($"Invalid Quantity {userCommand.Parameters[0]}");
                                }

                                return new SellCommand{GameState = gameState, StoreState = _storeState, ItemName = userCommand.Parameters[1].Trim(), Quantity = quantity};
                            }

                            return new SellCommand{GameState = gameState, StoreState = _storeState, ItemName = userCommand.Parameters[0].Trim() };
                        }
                    },
                    new CommandsDefinition
                    {
                        CommandText = "buy",
                        EntryDescription = "buy [quantity] [item name] || buy [item name] (to buy single item)",
                        Description = "Purchases the quantity indicated of the item requested.",
                        Command = (userCommand, gameState) => {

                            var command = new BuyCommand { GameState = gameState };
                            if(userCommand.Parameters.Count > 1)
                            {
                                command.ItemName = userCommand.Parameters[1];
                                command.NumOfItems = int.Parse(userCommand.Parameters[0]);
                            }
                            else
                            {
                                command.ItemName = userCommand.Parameters[0];
                                command.NumOfItems = 1;
                            }

                            return command;
                        }

                    },
                    new CommandsDefinition()
                    {
                        CommandText="stock",
                        Description = "Displays the items and quantities and unit prices available to purchase.",
                        Command = (userCommand, gameState) => new StockCommand { State = _gameState.Store.StoreState }
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "buying",
                        Description = "Displays the items the store is currently buying and the price paid per item.",
                        Command = (userCommand, _gameState) => new BuyingCommand { GameState = _gameState }
                    }
                }
            };

            return commandsGroup;
        }
    }
}