using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core
{
    public interface IPotatoChipGame
    {
        void StartGame();

        Scene CurrentScene { get; set; }
        ConsoleBuffer Output { get; set; }
        ConsoleBuffer Events { get; set; }
        Stack<Scene> SceneStack { get; }
        GameState GameState { get; }
        void ClearConsole(GameConsoles targetConsole = GameConsoles.Output);
    }
}