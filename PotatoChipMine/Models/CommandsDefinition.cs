using System;

namespace PotatoChipMine.Models
{
    public class CommandsDefinition
    {
        public string Command { get; set; }
        public string EntryDescription { get; set; }
        public string Description { get; set; }
        public Action<UserCommand,GameState> Execute { get; set; }
    }
}