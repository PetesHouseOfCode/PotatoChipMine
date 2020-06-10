using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class EquipCommand : CommandWithGameState, ICommand
    {
        public string DiggerName { get; set; }
        public int ClaimLeaseId { get; set; }
    }

    public class EquipCommandHandler : ICommandHandler<EquipCommand>
    {
        private GameState gameState;

        public void Handle(EquipCommand command)
        {
            gameState = command.GameState;

            var digger = gameState.Miner.Inventory("standard_digger");
            if (digger == null || digger.Count <= 0)
            {
                Game.WriteLine("You don't have any diggers in your inventory!", PcmColor.Red, null, GameConsoles.Input);
                return;
            }

            Game.PushScene(Scene.Create(new List<IGameEntity> {
                    new EquipHandlerEntity(gameState, command.DiggerName, command.ClaimLeaseId)
                }));
        }
    }
}
