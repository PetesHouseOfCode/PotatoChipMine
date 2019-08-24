using PotatoChipMine.Core.GameEngine;
using System;

namespace PotatoChipMine.Core.Commands
{
    public class BuyingCommand : CommandWithGameState, ICommand
    {
    }

    public class BuyingCommandHandler : ICommandHandler<BuyingCommand>
    {
        public void Handle(BuyingCommand command)
        {
            foreach (var itemBought in command.GameState.Store.StoreState.ItemsBuying)
            {
                Game.WriteLine($"Item Name:{itemBought.Name} Price Paid:{itemBought.Price} tt");
            }
        }
    }
}
