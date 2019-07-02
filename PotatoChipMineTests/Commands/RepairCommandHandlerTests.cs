using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMineTests.Helpers;
using PotatoChipMineTests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;

namespace PotatoChipMineTests.Commands
{
    [Collection("Game Tests")]
    public class RepairCommandHandlerTests
    {
        private const string DIGGER_NAME = "DiggerName";
        readonly MockMainProcess proc;
        readonly GameState gameState;
        readonly RepairCommandHandler repairCommandHandler = new RepairCommandHandler();

        public RepairCommandHandlerTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
        }

        [Fact]
        public void WithMissingDiggerThenReportDiggerMissing()
        {
            gameState.Miner = Miner.Default();
            
            var command = new RepairCommand
            {
                GameState = gameState,
                DiggerName = DIGGER_NAME,
            };

            repairCommandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"No digger named {DIGGER_NAME} could be found." + Environment.NewLine);

        }
    }
}
