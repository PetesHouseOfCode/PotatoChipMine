using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.Store.Services
{
    public class StoreCommandsGroupFactory
    {
        private readonly StoreState _storeState;
        private readonly GameState _gameState;

        public StoreCommandsGroupFactory(GameState gameState, StoreState storeState)
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
                            
                            // TODO: Check if no parameters
                            if(userCommand.Parameters.Count() == 0 || userCommand.Parameters.Count() > 2)
                            {
                                // return FailedMessageCommand{Message = $"Invalid Quantity {userCommand.Parameters[0]}"};
                            }

                            if(userCommand.Parameters.Count() == 2)
                            {
                                if(!int.TryParse(userCommand.Parameters[0], out var quantity))
                                {
                                    // return FailedMessageCommand{Message = $"Invalid Quantity {userCommand.Parameters[0]}"};
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
                        Execute = BuyHandler()

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
                        Execute = BuyingHandler()
                    }
                }
            };

            return commandsGroup;
        }

        private Action<UserCommand, GameState> BuyingHandler()
        {
            return (userCommand, gameState) =>
            {
                foreach (var itemBought in _storeState.ItemsBuying)
                {
                    Game.WriteLine($"Item Name:{itemBought.Name} Price Paid:{itemBought.Price} tt");
                }
            };
        }

        private Action<UserCommand, GameState> BuyHandler()
        {
            return (userCommand, gameState) =>
            {
                Game.WriteLine(Buy(
                            userCommand.Parameters.Count > 1
                                ? userCommand.Parameters[1]
                                : userCommand.Parameters[0]
                            , userCommand.Parameters.Count == 1 ? 1 : int.Parse(userCommand.Parameters[0]))
                        .message);
            };
        }

        public (bool sold, string message) Buy(string itemName, int quantity)
        {
            var item = _gameState.Store.StoreState.ItemsForSale.FirstOrDefault(x => x.Name.ToLower() == itemName.ToLower());
            if (item == null) return (false, $"We do not carry {itemName}.  Try MINER-MART.");
            if (item.Count - quantity < 0 || quantity < 1)
                return (false, $"We do not currently have {quantity} of {itemName} in stock.");
            if ((quantity * item.Price) > _gameState.Miner.TaterTokens)
                return (false, "You don't have enough tater tokens to make that purchase");
            _gameState.Miner.TaterTokens = _gameState.Miner.TaterTokens - (quantity * item.Price);
            var stack = _gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name == item.Name);
            if (stack != null)
            {
                stack.Count += quantity;

            }
            else
            {
                _gameState.Miner.InventoryItems.Add(new InventoryItem { ItemId = item.ItemId, Name = item.Name, Count = quantity,Description = item.Description});
            }

            item.Count -= quantity;
            return (true, $"{quantity} {item.Name} have been added to your inventory");
        }
    }
}