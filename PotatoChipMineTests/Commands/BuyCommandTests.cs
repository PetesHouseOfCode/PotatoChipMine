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
        const string GAME_ITEM_NAME = "GameItem";
        const string GAME_ITEM_NAME_PLURALIZED = "GameItems";

        public BuyCommandTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            gameState.Miner = Miner.Default();
            gameState.Store = gameState.Store ?? new MinerStoreFactory(gameState, CommandsGroup.Empty(), proc.Gateway).Build();
        }

        [Fact]
        public void Buying_quantity_of_zero_or_less_should_fail()
        {
            AddItemToStore();

            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = 0,
                ItemName = GAME_ITEM_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"Invalid Quantity.");
        }

        [Fact]
        public void Shop_has_to_carry_the_item_to_complete_the_purchase()
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
        public void Item_is_available_in_the_shop_to_complete_the_purchase()
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
        public void Miner_needs_enough_tater_tokens_to_complete_purchase()
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

        [Fact]
        public void With_enough_tokens_and_item_in_store_complete_purchase_of_more_than_one_with_pluralized_name()
        {
            AddItemToStore();
            AddCountToItemInStore(2);

            var buyCount = 2;
            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = buyCount,
                ItemName = GAME_ITEM_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"{buyCount} {GAME_ITEM_NAME_PLURALIZED} have been added to your inventory");
        }

        [Fact]
        public void With_enough_tokens_and_item_in_store_complete_purchase_with_pluralized_name()
        {
            AddItemToStore();
            AddCountToItemInStore(1);

            var buyCount = 1;
            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = buyCount,
                ItemName = GAME_ITEM_NAME_PLURALIZED
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            gameState.Miner.InventoryItems.Count.ShouldBe(2);
            output.ShouldBe($"{buyCount} {GAME_ITEM_NAME} have been added to your inventory");
        }

        [Fact]
        public void With_enough_tokens_and_item_in_store_complete_purchase_with_pluralized_name_and_singular_name()
        {
            AddItemToStore();
            AddCountToItemInStore(2);

            var buyCount = 1;
            var command = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = buyCount,
                ItemName = GAME_ITEM_NAME_PLURALIZED
            };
            commandHandler.Handle(command);
            ConsoleBufferHelper.GetText(proc.Output);

            var commandSingle = new BuyCommand
            {
                GameState = gameState,
                NumOfItems = buyCount,
                ItemName = GAME_ITEM_NAME
            };
            commandHandler.Handle(commandSingle);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            gameState.Miner.InventoryItems.Count.ShouldBe(2);
            var inventoryItem = gameState.Miner.Inventory(GAME_ITEM_NAME);
            inventoryItem.Count.ShouldBe(2);
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
                                    Id = GAME_ITEM_ID,
                                    Name = GAME_ITEM_NAME,
                                    PluralizedName = GAME_ITEM_NAME_PLURALIZED
                                }
                            });
        }
    }
}
