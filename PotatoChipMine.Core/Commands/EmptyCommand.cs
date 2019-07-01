using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PotatoChipMine.Core.Commands
{
    public class EmptyCommand : CommandWithGameState, ICommand
    {
        public string DiggerName { get; set; }
    }

    public class EmptyCommandHandler : ICommandHandler<EmptyCommand>
    {
        public void Handle(EmptyCommand command)
        {
            var gameState = command.GameState;

            var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                string.Equals(x.Name, command.DiggerName, StringComparison.CurrentCultureIgnoreCase));
            var chips = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name == "chips");

            if (chips == null)
            {
                Game.WriteLine($"Could not find Chips in the inventory", PcmColor.Red);
                return;
            }

            if (digger == null)
            {
                Game.WriteLine($"Could not find digger named {command.DiggerName}", PcmColor.Red);
                return;
            }

            var hopperCount = digger.Hopper.Empty();
            chips.Count += hopperCount;
            gameState.Miner.UpdateLifetimeStat(Stats.LifetimeChips, hopperCount);
            Game.WriteLine($"{hopperCount} was removed from {command.DiggerName}'s hopper and moved into the chip vault.",
                PcmColor.Yellow);
            Game.WriteLine($"Vault Chips:{gameState.Miner.Inventory("chips").Count}", PcmColor.Yellow);
        }
    }
}
