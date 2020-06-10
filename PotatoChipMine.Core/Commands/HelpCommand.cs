using PotatoChipMine.Core.GameEngine;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class HelpCommand : CommandWithGameState, ICommand
    {
    }

    public class HelpCommandHandler : ICommandHandler<HelpCommand>
    {
        public void Handle(HelpCommand command)
        {
            var gameState = command.GameState;

            Game.WriteLine($"-----------   {gameState.Mode.ToString().ToUpper()} Commands  ---------------");

            foreach (var commandsDefinition in gameState.CurrentRoom.CommandsGroup.LocalCommands.OrderBy(x => x.CommandText))
            {
                var commandName = commandsDefinition.EntryDescription ?? commandsDefinition.CommandText;
                Game.WriteLine($"Command: [{commandName}]");
                Game.WriteLine($"Description: {commandsDefinition.Description}");
                Game.WriteLine("--------");
            }
        }
    }
}
