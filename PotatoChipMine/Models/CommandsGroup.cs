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
            var command = LocalCommands.FirstOrDefault(x =>
                x.Command.Trim().ToLower().Equals(userCommand.CommandText.Trim().ToLower()));
            if (command == null)
            {
                GameUi.ReportBadCommand(userCommand.CommandText);
            }
            command?.Execute(userCommand, gameState);
        }
    }
}