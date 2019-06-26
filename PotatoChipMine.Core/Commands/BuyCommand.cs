using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var item = gameState.Store.StoreState.ItemsForSale.FirstOrDefault(x => x.Name.ToLower() == itemName.ToLower());
            if (item == null) return (false, $"We do not carry {itemName}.  Try MINER-MART.");
            if (item.Count - quantity < 0 || quantity < 1)
                return (false, $"We do not currently have {quantity} of {itemName} in stock.");
            if ((quantity * item.Price) > gameState.Miner.TaterTokens)
                return (false, "You don't have enough tater tokens to make that purchase");
            gameState.Miner.TaterTokens = gameState.Miner.TaterTokens - (quantity * item.Price);
            var stack = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name == item.Name);
            if (stack != null)
            {
                stack.Count += quantity;

            }
            else
            {
                gameState.Miner.InventoryItems.Add(new InventoryItem { ItemId = item.ItemId, Name = item.Name, Count = quantity, Description = item.Description });
            }

            item.Count -= quantity;
            return (true, $"{quantity} {item.Name} have been added to your inventory");
        }
    }
}
