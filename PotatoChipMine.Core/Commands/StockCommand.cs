using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class StockCommand : ICommand
    {
        public StoreState State { get; set; }
    }

    public class StockCommandHandler : ICommandHandler<StockCommand>
    {
        public void Handle(StockCommand command)
        {
            var table = new TableOutput(80, PcmColor.Green);
            table.AddHeaders("Name", "Price", "Quantity");
            foreach (var storeItem in command.State.ItemsForSale)
            {
                table.AddRow(storeItem.Name, storeItem.Price.ToString(), storeItem.Count.ToString());
            }

            Game.Write(table);
        }
    }
}
