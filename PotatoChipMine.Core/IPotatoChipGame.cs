using PotatoChipMine.Core.GameEngine;
using System;
using System.Collections.Generic;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core
{
    public interface IPotatoChipGame
    {
        void StartGame();

        Scene CurrentScene { get; set; }
        ConsoleBuffer Output { get; set; }
        ConsoleBuffer Events { get; set; }
        Stack<Scene> SceneStack { get; }
        void ClearConsole(GameConsoles targetConsole = GameConsoles.Output);
    }
}