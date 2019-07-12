using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public abstract class CommandWithGameState : ICommand
    {
        public GameState GameState { get; set; }
    }
}
