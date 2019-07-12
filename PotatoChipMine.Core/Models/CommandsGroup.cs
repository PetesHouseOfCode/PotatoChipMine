using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Models
{
    public class CommandsGroup
    {
        public List<CommandsDefinition> LocalCommands { get; set; } = new List<CommandsDefinition>();

        public void ExecuteCommand(UserCommand userCommand, GameState gameState)
        {
            var command = LocalCommands.FirstOrDefault(x =>
                x.CommandText.Trim().ToLower().Equals(userCommand.CommandText.Trim().ToLower()));
            if (command == null)
            {
                Game.WriteLine($"{userCommand.CommandText} is not a valid command.", PcmColor.Red,null,GameConsoles.Input);
                Game.WriteLine("Type [help] to see a list of commands.", PcmColor.Red,null,GameConsoles.Input);
                return;
            }

            if (command.Command == null)
            {
                command.Execute(userCommand, gameState);
                return;
            }

            CommandRunner.Run(command.Command(userCommand, gameState));
        }

        public CommandsGroup Join(CommandsGroup joinedCommandsGroup)
        {
            var commandsGroup = new CommandsGroup();
            commandsGroup.LocalCommands.AddRange(this.LocalCommands);
            commandsGroup.LocalCommands.AddRange(joinedCommandsGroup.LocalCommands);
            return commandsGroup;
        }
    }
}