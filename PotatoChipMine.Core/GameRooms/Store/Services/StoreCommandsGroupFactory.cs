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
                        Command = "sell",
                        EntryDescription = "sell [quantity] [item name] || sell [item name] (sells all) ",
                        Description = "Sell all of the indicated items in your inventory.",
                        Execute = SellHandler()
                    },
                    new CommandsDefinition
                    {
                        Command = "buy",
                        EntryDescription = "buy [quantity] [item name] || buy [item name] (to buy single item)",
                        Description = "Purchases the quantity indicated of the item requested.",
                        Execute = BuyHandler()

                    },
                    new CommandsDefinition()
                    {
                        Command="stock",
                        Description = "Displays the items and quantities and unit prices available to purchase.",
                        Execute = StockHandler()
                    },
                    new CommandsDefinition()
                    {
                        Command = "buying",
                        Description = "Displays the items the store is currently buying and the price paid per item.",
                        Execute = BuyingHandler()
                    }
                }
            };

            //commandsGroup.LocalCommands.Add(new CommandsDefinition()
            //{
            //    Command = "help",
            //    Description = "Shows a description of all the currently available commands.",
            //    Execute = (userCommand, gameState) =>
            //    {
            //        Game.WriteLine($"-----------   {gameState.Mode.ToString().ToUpper()} Commands  ---------------");

            //        foreach (var commandsDefinition in gameState.CurrentRoom.CommandsGroup.LocalCommands.OrderBy(x => x.Command))
            //        {
            //            var command = commandsDefinition.EntryDescription ?? commandsDefinition.Command;
            //            Game.WriteLine($"Command: [{command}]");
            //            Game.WriteLine($"Description: {commandsDefinition.Description}");
            //            Game.WriteLine("--------");
            //        }
            //    }
            //});
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

        private Action<UserCommand, GameState> StockHandler()
        {
            return (userCommand, gameState) =>
            {
                var table = new TableOutput(80, PcmColor.Green);
                table.AddHeaders("Name", "Price", "Quantity");
                foreach (var storeItem in _storeState.ItemsForSale)
                {
                    table.AddRow(storeItem.Name, storeItem.Price.ToString(), storeItem.Count.ToString());
                }

                Game.Write(table);
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

        private Action<UserCommand, GameState> SellHandler()
        {
            return (userCommand, gameState) =>
            {
                var result = Sell(userCommand.Parameters);
                if (!result.sold)
                {
                    Game.WriteLine(result.message, PcmColor.Red);
                    return;
                }

                Game.WriteLine(result.message);
            };
        }

        private (bool sold, string message) Sell(IReadOnlyList<string> paramsList)
        {
            try
            {
                int quantity;
                InventoryItem item;
                if (paramsList.Count >= 2)
                {
                    if (!int.TryParse(paramsList[0], out quantity))
                    {
                        return (false, "Sell command was not in the correct format.");
                    }

                    item = _gameState.Miner.Inventory(paramsList[1]);
                    if (item == null)
                    {
                        return (false, $"You don't have any {paramsList[0]} to sell");
                    }
                }
                else
                {

                    item = _gameState.Miner.Inventory(paramsList[0]);
                    if (item == null)
                    {
                        return (false, $"You don't have any {paramsList[0]} to sell");
                    }

                    quantity = item.Count;
                }

                var price = _storeState.ItemsBuying.Any(x => x.Name.ToLower() == item.Name)
                    ? _storeState.ItemsBuying.First(x => x.Name.ToLower() == item.Name).Price
                    : 1;
                item.Count -= quantity;
                _gameState.Miner.TaterTokens += quantity * price;
                _gameState.Miner.LifetimeTokens += quantity * price;
                return (true, $"Sold {quantity} chips for {quantity * price}.");
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, "Invalid entry. Indicate an item to sell.");
            }

        }

        public (bool sold, string message) Buy(string itemName, int quantity)
        {
            var item = _storeState.ItemsForSale.FirstOrDefault(x => x.Name.ToLower() == itemName.ToLower());
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
                _gameState.Miner.InventoryItems.Add(new InventoryItem { ItemId = item.ItemId, Name = item.Name, Count = quantity });
            }

            item.Count -= quantity;
            return (true, $"{quantity} {item.Name} have been added to your inventory");
        }
    }
}