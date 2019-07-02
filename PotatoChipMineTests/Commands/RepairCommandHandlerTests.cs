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
using System.Linq;

namespace PotatoChipMineTests.Commands
{
    [Collection("Game Tests")]
    public class RepairCommandHandlerTests
    {
        private const string MISSING_DIGGER_NAME = "MissingDiggerName";
        private const string EXISTING_DIGGER_NAME = "ExistingDiggerName";
        
        readonly MockMainProcess proc;
        readonly GameState gameState;
        readonly RepairCommandHandler repairCommandHandler = new RepairCommandHandler();

        public RepairCommandHandlerTests()
        {
            proc = new MockMainProcess();
            gameState = new GameState();
            Game.SetMainProcess(proc);
            gameState.Miner = Miner.Default();
            gameState.Miner.InventoryItems.Add(new InventoryItem { Name = "bolts", Count = 10000 });

            var digger = ChipDigger.StandardDigger(new MineSite
            {
                ChipDensity = PotatoChipMine.Core.ChipDensity.Normal,
                Hardness = SiteHardness.Firm
            });
            digger.Name = EXISTING_DIGGER_NAME;

            gameState.Miner.Diggers.Add(digger);

        }

        [Fact]
        public void WithMissingDiggerThenReportDiggerMissing()
        {
            var command = new RepairCommand
            {
                GameState = gameState,
                DiggerName = MISSING_DIGGER_NAME,
            };

            repairCommandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"No digger named {MISSING_DIGGER_NAME} could be found." + Environment.NewLine);
        }

        [Fact]
        public void WithoutTokensThenReportShortOnTokens()
        {
            gameState.Miner.TaterTokens = 0;
            var command = new RepairCommand
            {
                GameState = gameState,
                DiggerName = EXISTING_DIGGER_NAME,
            };

            repairCommandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            var outputLines = output.Split(Environment.NewLine);

            outputLines[0].ShouldStartWith("Repairs will cost");
            outputLines[1].ShouldBe("You don't have enough tokens.");
            outputLines.Length.ShouldBe(3);
        }
    }
}
