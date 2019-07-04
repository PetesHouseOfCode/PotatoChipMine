using PotatoChipMine.Core;
using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMineTests.Helpers;
using PotatoChipMineTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using System.Linq;

namespace PotatoChipMineTests.Entities
{
    [Collection("Game Tests")]
    public class CollectMinerNameEntityTests
    {
        const string MINER_NAME = "MinerName";
        readonly MockMainProcess proc;
        readonly GameState gameState;
        CollectMinerNameEntity entity;

        public CollectMinerNameEntityTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            entity = new CollectMinerNameEntity(gameState);
        }

        [Fact]
        public void WhenEntityStartsDisplayHowdyMessageAndSetPromptToEnterName()
        {
            entity.Update(Frame.NewFrame(TimeSpan.Zero, TimeSpan.Zero));
            var data = ConsoleBufferHelper.GetLines(proc.Output);

            data[0].ShouldBe(@"Howdy pilgrim!  Welcome to glamorous world of 'tater chip mining!");
            data[1].ShouldBe("I'm Earl, your mine bot. I'll be you're right hand man ... 'er bot, around this here mining operation.");
            data[2].ShouldBe("Whats your name pilgrim?");

            data.Count.ShouldBe(3);
            gameState.PromptText.ShouldBe("Enter your name:");
        }

        [Fact]
        public void WhenEntityHasAlreadySentMessageItShouldNotShowAgain()
        {
            entity.Update(Frame.NewFrame(TimeSpan.Zero, TimeSpan.Zero));
            string data = ConsoleBufferHelper.GetText(proc.Output);

            entity.Update(Frame.NewFrame(TimeSpan.Zero, TimeSpan.Zero));
            data = ConsoleBufferHelper.GetText(proc.Output);

            data.ShouldBeEmpty();
        }

        [Fact]
        public void WhenInputIsEmptyWriteErrorMessage()
        {
            entity.HandleInput(new UserCommand());
            var data = ConsoleBufferHelper.GetText(proc.Output);
            data.ShouldBe("Please enter a name.");
        }

        [Fact]
        public void WhenInputIsNotEmptySetMinerNameToEntry()
        {
            entity.HandleInput(new UserCommand { FullCommand = MINER_NAME });

            gameState.Miner.Name.ShouldBe(MINER_NAME);
            gameState.PromptText.ShouldBeNull();

            proc.CurrentScene.Entities.First().ShouldBeOfType(typeof(WelcomeEntity));
        }
    }
}
