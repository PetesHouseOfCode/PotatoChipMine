using PotatoChipMine.Core.GameEngine;
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
    }
}