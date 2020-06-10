using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using PotatoChipMineTests.Mocks;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Services;

namespace PotatoChipMineTests
{
    public class MinerStoreFactoryTests
    {
        MockMainProcess proc;
        GameState gameState;
        MinerStoreFactory minerStore;

        public MinerStoreFactoryTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            gameState.Miner = Miner.Default();
            minerStore = new MinerStoreFactory(gameState, CommandsGroup.Empty(), proc.Gateway);
        }

        [Fact]
        public void New_miner_store_should_have_items_it_buys()
        {
            var store = minerStore.Build();
            store.StoreState.ItemsBuying.ShouldNotBeEmpty();
        }

        [Fact]
        public void New_miner_store_should_have_items_it_sells()
        {
            var store = minerStore.Build();
            store.StoreState.ItemsForSale.ShouldNotBeEmpty();
        }
    }
}
