using PotatoChipMine.Core;
using PotatoChipMine.Core.Data;
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
        public GameState GameState { get; } = new GameState();
        public DataGateway Gateway { get; } = new DataGateway(null, new MockGameItemRepository(), null);

        public void ClearConsole(GameConsoles targetConsole = GameConsoles.Output)
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }

    public class MockGameItemRepository : IRepository<GameItem>
    {
        public IReadOnlyList<GameItem> GetAll()
        {
            return new List<GameItem>
            {
                new GameItem() { Id = 1, Name = "Standard_Digger"},
                new GameItem() { Id = 2, Name = "Bolts"},
                new GameItem() { Id = 3, Name = "RawChips"},
                new GameItem() { Id = 4, Name = "Chip"}
            };
        }
    }
}
