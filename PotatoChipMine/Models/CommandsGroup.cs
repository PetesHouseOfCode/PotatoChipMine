using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Services;

namespace PotatoChipMine.Models
{
    public class CommandsGroup
    {
        protected internal GameUI GameUi;
        public CommandsGroup ParentGroup { get; set; }
        public List<CommandsDefinition> LocalCommands { get; set; }
        public virtual void ExecuteCommand(GameUI gameUi, UserCommand userCommand,GameState gameState)
        {
            GameUi = gameUi;
            var command = ParentGroup?.LocalCommands.FirstOrDefault(x =>
                x.Command.ToLower().Equals(userCommand.CommandText.ToLower()));
            if (command != null && command.Command != "help")
            {
                command.Execute(userCommand, gameState);
                return;
            }

            command = LocalCommands.FirstOrDefault(x =>
                x.Command.Trim().ToLower().StartsWith(userCommand.CommandText.Trim().ToLower()));
            if (command == null)
            {
                GameUi.ReportBadCommand(userCommand.CommandText);
            }
            command?.Execute(userCommand, gameState);
        }
    }
}