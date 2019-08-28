using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMineTests.Helpers;
using PotatoChipMineTests.Mocks;
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

            var digger = ChipDigger.StandardDigger(new MineSite
                {
                    ChipDensity = PotatoChipMine.Core.ChipDensity.Normal,
                    Hardness = SiteHardness.Firm
                });
            digger.Name = EXISTING_DIGGER_NAME;

            gameState.Miner.Diggers.Add(digger);
        }

        [Fact]
        public void Digger_with_name_has_to_exist()
        {
            var command = new RepairCommand
            {
                GameState = gameState,
                DiggerName = MISSING_DIGGER_NAME,
            };

            repairCommandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetText(proc.Output);
            output.ShouldBe($"No digger named {MISSING_DIGGER_NAME} could be found.");
        }

        [Fact]
        public void Need_enough_tokens_to_repair()
        {
            gameState.Miner.InventoryItems.Add(new InventoryItem
            {
                Count = 10000,
                Item = new GameItem
                {
                    Name = "bolts"
                }
            });
            gameState.Miner.TaterTokens = 0;
            var command = new RepairCommand
            {
                GameState = gameState,
                DiggerName = EXISTING_DIGGER_NAME,
            };

            repairCommandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetLines(proc.Output);

            output[0].ShouldStartWith("Repairs will cost");
            output[1].ShouldBe("You don't have enough tokens.");
            output.Count().ShouldBe(2);
        }

        [Fact]
        public void Need_enough_bolts_to_repair()
        {
            var command = new RepairCommand
            {
                GameState = gameState,
                DiggerName = EXISTING_DIGGER_NAME,
            };

            repairCommandHandler.Handle(command);

            var output = ConsoleBufferHelper.GetLines(proc.Output);

            output[0].ShouldStartWith("Repairs will cost");
            output[1].ShouldBe("You don't have enough bolts.");
            output.Count().ShouldBe(2);
        }
    }
}
