using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class SellCommand : CommandWithGameState
    {
        public StoreState StoreState { get; set; }
        public int? Quantity { get; set; }
        public string ItemName { get; set; }
    }

    public class SellCommandHandler : ICommandHandler<SellCommand>
    {
        GameState gameState;
        StoreState storeState;

        public void Handle(SellCommand command)
        {
            gameState = command.GameState;
            storeState = command.StoreState;

            var result = Sell(command.ItemName, command.Quantity);

            if (!result.sold)
            {
                Game.WriteLine(result.message, PcmColor.Red);
                return;
            }

            Game.WriteLine(result.message);
        }

        private (bool sold, string message) Sell(string itemName, int? quantity)
        {
            try
            {
                InventoryItem item;
                if (quantity.HasValue)
                {
                    item = gameState.Miner.Inventory(itemName);
                    if (item == null)
                    {
                        return (false, $"You don't have any {itemName} to sell");
                    }
                }
                else
                {
                    item = gameState.Miner.Inventory(itemName);
                    if (item == null)
                    {
                        return (false, $"You don't have any {itemName} to sell");
                    }

                    quantity = item.Count;
                }

                var price = storeState.ItemsBuying.Any(x => x.Name.ToLower() == item.Name)
                    ? storeState.ItemsBuying.First(x => x.Name.ToLower() == item.Name).Price
                    : 1;
                item.Count -= quantity.Value;
                var tokenChange = quantity.Value * price;
                gameState.Miner.TaterTokens += tokenChange;
                gameState.Miner.UpdateLifetimeStat(Stats.LifetimeTokens, tokenChange);
                return (true, $"Sold {quantity} chips for {quantity.Value * price}.");
            }
            catch (ArgumentOutOfRangeException)
            {
                return (false, "Invalid entry. Indicate an item to sell.");
            }

        }
    }
}
