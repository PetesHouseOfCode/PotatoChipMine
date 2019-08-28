using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class EquipCommand : CommandWithGameState, ICommand
    {
    }

    public class EquipCommandHandler : ICommandHandler<EquipCommand>
    {
        private GameState gameState;

        public void Handle(EquipCommand command)
        {
            gameState = command.GameState;

            var scene = Scene.Create(new List<IGameEntity>{
                    new EquipHandlerEntity(gameState)
                });

            var digger = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name.ToLower() == "digger");
            if (digger != null && digger.Count > 0)
            {
                gameState.PromptText = "Enter Digger Name: ";
                Game.PushScene(scene);
                return;
            }

            Game.WriteLine("You don't have any diggers in your inventory!", PcmColor.Red, null, GameConsoles.Input);
        }
    }
}
