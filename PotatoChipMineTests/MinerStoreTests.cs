using PotatoChipMine.Core.GameRooms.Store;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PotatoChipMineTests
{
    public class MinerStoreTests
    {
        [Fact]
        public void MinerStore()
        {
            var gameState = new GameState();
            var store = new MinerStore(gameState, new string[] { }, new CommandsGroup());
        }
    }
}
