using System;
using System.Collections.Generic;
using System.Text;
using PotatoChipMine;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.Store;
using PotatoChipMine.Store.Models;
using Xunit;

namespace PotatoChipMineTests
{
    public class MinerStoreTests
    {
        [Fact]
        public void MinerStore()
        {
            var store = new MinerStore(new GameUI(),new GameState(),new StoreState(), new string[]{});

        }
    }
}
