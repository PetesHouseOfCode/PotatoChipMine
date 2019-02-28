using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Services;

namespace PotatoChipMine.Models
{
    public class CommandsGroup
    {
        protected internal GameUI GameUi;
        public List<CommandsDefinition> LocalCommands { get; set; } = new List<CommandsDefinition>();

        public virtual void ExecuteCommand(GameUI gameUi, UserCommand userCommand, GameState gameState)
        {
            GameUi = gameUi;
            var command = LocalCommands.FirstOrDefault(x =>
                x.Command.Trim().ToLower().Equals(userCommand.CommandText.Trim().ToLower()));
            if (command == null)
            {
                GameUi.ReportBadCommand(userCommand.CommandText);
            }
            command?.Execute(userCommand, gameState);
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