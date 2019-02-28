using System;
using System.Collections.Generic;
using System.Text;
using PotatoChipMine;
using PotatoChipMine.GameRooms.Store;
using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using Xunit;

namespace PotatoChipMineTests
{
    public class MinerStoreTests
    {
        [Fact]
        public void MinerStore()
        {
            var gameState = new GameState();
            var store = new MinerStore(new GameUI(gameState), gameState, new string[] { }, new CommandsGroup());
        }
    }
}
