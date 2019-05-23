using PotatoChipMine.Core.Commands;
using System;

namespace PotatoChipMine.Core.Models
{
    public class CommandsDefinition
    {
        public string CommandText { get; set; }
        public string EntryDescription { get; set; }
        public string Description { get; set; }
        public Action<UserCommand, GameState> Execute { get; set; }
        public Func<UserCommand, GameState, ICommand> Command { get; set; }
    }
}