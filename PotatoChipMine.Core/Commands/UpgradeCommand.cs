using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Commands
{
    public class UpgradeCommand : CommandWithGameState, ICommand
    {

    }

    public class UpgradeCommandHandler : ICommandHandler<UpgradeCommand>
    {
        public void Handle(UpgradeCommand command)
        {
            var gameState = command.GameState;

            var scene = Scene.Create(new List<IGameEntity>
                {
                    new UpgradeHandlerEntity(gameState)
                });

            gameState.PromptText = "Enter Digger Name: ";
            Game.PushScene(scene);
            return;
        }
    }
}
