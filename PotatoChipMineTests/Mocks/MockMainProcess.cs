using PotatoChipMine.Core;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMineTests.Mocks
{
    public class MockMainProcess : IPotatoChipGame
    {
        public Scene CurrentScene { get; set; }
        public ConsoleBuffer Output { get; set; } = new ConsoleBuffer();
        public ConsoleBuffer Events { get; set; } = new ConsoleBuffer();
        public Stack<Scene> SceneStack { get; } = new Stack<Scene>();

        public void ClearConsole(GameConsoles targetConsole = GameConsoles.Output)
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }
}