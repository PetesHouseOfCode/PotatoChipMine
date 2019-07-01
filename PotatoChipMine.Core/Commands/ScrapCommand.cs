using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PotatoChipMine.Core.Commands
{
    public class ScrapCommand : CommandWithGameState, ICommand
    {
        public string DiggerName { get; set; }
    }

    public class ScrapCommandHandler : ICommandHandler<ScrapCommand>
    {
        public void Handle(ScrapCommand command)
        {
            var gameState = command.GameState;

            var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                string.Equals(x.Name, command.DiggerName, StringComparison.CurrentCultureIgnoreCase));
            if (digger == null)
            {
                Game.WriteLine($"There are no diggers named {command.DiggerName}.", PcmColor.Red);
                return;
            }
            gameState.Miner.Diggers.Remove(digger);
            var bolts = gameState.Miner.Inventory("bolts");
            if (bolts == null)
            {
                bolts = new InventoryItem() { Name = "bolts", Count = 0 };
                gameState.Miner.InventoryItems.Add(bolts);
            }

            var boltsReceived = new Random().Next(3, 10);
            bolts.Count += boltsReceived;
            Game.WriteLine($"{digger.Name} was scrapped for {boltsReceived} bolts.", PcmColor.Yellow);
        }
    }
}
