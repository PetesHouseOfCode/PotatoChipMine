using PotatoChipMine.Core;
using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;

using System;
using System.Collections.Generic;

namespace PotatoChipMineTests.Mocks
{
    public class MockMainProcess : IPotatoChipGame
    {
        public Scene CurrentScene { get; set; }
        public ConsoleBuffer Output { get; set; } = new ConsoleBuffer();
        public ConsoleBuffer Events { get; set; } = new ConsoleBuffer();
        public Stack<Scene> SceneStack { get; } = new Stack<Scene>();
        public GameState GameState { get; } = new GameState();
        public DataGateway Gateway { get; } = new DataGateway(null, new MockGameItemRepository(), null, new MockStoreInventoryRepository());

        public void ClearConsole(GameConsoles targetConsole = GameConsoles.Output)
        {
            throw new NotImplementedException();
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }

    public class MockStoreInventoryRepository : IRepository<StoreItem>
    {
        public IReadOnlyList<StoreItem> GetAll()
        {
            return new List<StoreItem>
            {
                new StoreItem { BuyingPrice = 20, GameItemId = 4 },
                new StoreItem { SellingPrice = 20, GameItemId = 1 }
            };
        }
    }

    public class MockGameItemRepository : IRepository<GameItem>
    {
        public IReadOnlyList<GameItem> GetAll()
        {
            return new List<GameItem>
            {
                new GameItem() { Id = 1, Name = "Standard_Digger", PluralizedName = "Standard_Diggers"},
                new GameItem() { Id = 2, Name = "Bolt", PluralizedName = "Bolts"},
                new GameItem() { Id = 3, Name = "RawChip", PluralizedName = "RawChips"},
                new GameItem() { Id = 4, Name = "Chip", PluralizedName = "Chip"}
            };
        }
    }
}
