using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMineTests.Helpers;
using PotatoChipMineTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PotatoChipMineTests.Commands
{
    [Collection("Game Tests")]
    public class EmptyCommandTests
    {
        private const string MISSING_DIGGER_NAME = "MissingDiggerName";
        private const string EXISTING_DIGGER_NAME = "ExistingDiggerName";

        readonly MockMainProcess proc;
        readonly GameState gameState;
        readonly EmptyCommandHandler commandHandler = new EmptyCommandHandler();

        public EmptyCommandTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            gameState.Miner = Miner.Default();

            var digger = ChipDigger.StandardDigger(new MineClaim
            {
                ChipDensity = PotatoChipMine.Core.ChipDensity.Normal,
                Hardness = SiteHardness.Firm
            });
            digger.Name = EXISTING_DIGGER_NAME;

            gameState.Miner.Diggers.Add(digger);
        }
        
        [Fact]
        public void Digger_name_that_doesnt_exist_is_invalid()
        {
            var command = new EmptyCommand
            {
                GameState = gameState,
                DiggerName = MISSING_DIGGER_NAME
            };

            commandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"Could not find digger named {MISSING_DIGGER_NAME}");
        }
    }
}
