using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class BuyCommand : CommandWithGameState, ICommand
    {
        public string ItemName { get; set; }
        public int NumOfItems { get; set; }
    }

    public class BuyCommandHandler : ICommandHandler<BuyCommand>
    {
        Models.GameState gameState;
        public void Handle(BuyCommand command)
        {
            gameState = command.GameState;
            Game.WriteLine(Buy(command.ItemName, command.NumOfItems).message);
        }

        public (bool sold, string message) Buy(string itemName, int quantity)
        {
            var storeItem = gameState.Store.StoreState.ItemsForSale.FirstOrDefault(x => x.Name.ToLower() == itemName.ToLower());
            if (storeItem == null) return (false, $"We do not carry {itemName}.  Try MINER-MART.");
            if (storeItem.Count - quantity < 0 || quantity < 1)
                return (false, $"We do not currently have {quantity} of {itemName} in stock.");
            if ((quantity * storeItem.Price) > gameState.Miner.TaterTokens)
                return (false, "You don't have enough tater tokens to make that purchase");
            gameState.Miner.TaterTokens = gameState.Miner.TaterTokens - (quantity * storeItem.Price);
            var stack = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Item.Name == storeItem.Name);
            if (stack != null)
            {
                stack.Count += quantity;

            }
            else
            {
                var inventoryItem = new InventoryItem
                {
                    Count = quantity,
                    Item = storeItem.Item
                };

                gameState.Miner.InventoryItems.Add(inventoryItem);
            }

            storeItem.Count -= quantity;
            return (true, $"{quantity} {storeItem.Name} have been added to your inventory");
        }
    }
}
