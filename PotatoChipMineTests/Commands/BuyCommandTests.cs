using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMineTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using PotatoChipMine.Core.Commands;
using PotatoChipMineTests.Helpers;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.GameRooms.Store;
using PotatoChipMine.Core.GameRooms.Store.Services;
using System.Linq;

namespace PotatoChipMineTests.Commands
{
    [Collection("Game Tests")]
    public class BuyCommandTests
    {
        MockMainProcess proc;
        GameState gameState;
        BuyCommandHandler commandHandler = new BuyCommandHandler();

        const int GAME_ITEM_ID = 1;
        const string GAME_ITEM_NAME = "GameItems";

        public BuyCommandTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            gameState.Miner = Miner.Default();
            gameState.Store = gameState.Store ?? new MinerStoreFactory(gameState, CommandsGroup.Empty()).BuildMineStore();
        }

        [Fact]
        public void Need_to_carry_the_item_to_complete_the_purchase()
        {
            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = 2,
                ItemName = GAME_ITEM_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"We do not carry {GAME_ITEM_NAME}.  Try MINER-MART.");
        }

        [Fact]
        public void Need_to_have_item_available_in_the_shop_to_complete_the_purchase()
        {
            AddItemToStore();

            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = 1,
                ItemName = GAME_ITEM_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"We do not currently have 1 of {GAME_ITEM_NAME} in stock.");
        }
        
        [Fact]
        public void Need_to_have_enough_tater_tokens()
        {
            AddItemToStore();
            AddCountToItemInStore(1);
            EmptyMinerTokens();

            var buyCount = 1;
            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = buyCount,
                ItemName = GAME_ITEM_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe("You don't have enough tater tokens to make that purchase");
        }
        
        [Fact]
        public void With_enough_tokens_and_item_in_store_complete_purchase()
        {
            AddItemToStore();
            AddCountToItemInStore(1);

            var buyCount = 1;
            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = buyCount,
                ItemName = GAME_ITEM_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"{buyCount} {GAME_ITEM_NAME} have been added to your inventory");
        }

        private void EmptyMinerTokens()
        {
            gameState.Miner.TaterTokens = 0;
        }

        private void AddCountToItemInStore(int count)
        {
            gameState.Store.StoreState.ItemsForSale.First(x => x.Name == GAME_ITEM_NAME).Count = count;
        }

        private void AddItemToStore()
        {
            gameState.Store.StoreState.ItemsForSale.Add(
                            new StoreItem
                            {
                                Count = 0,
                                Price = 1,
                                Item = new GameItem
                                {
                                    ItemId = GAME_ITEM_ID,
                                    Name = GAME_ITEM_NAME
                                }
                            });
        }
    }
}
