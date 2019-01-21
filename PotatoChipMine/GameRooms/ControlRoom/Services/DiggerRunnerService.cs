using System;
using System.Threading;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom.Services
{
    public class DiggerRunnerService
    {
        public static void RunDiggers(GameUI gameUi, GameState gameState, int turns = 5)
        {
            var miner = gameState.Miner;
            gameUi.ReportDiggersStarting(miner.Diggers);
            gameUi.ReportInfo(new[] {"Goood luuuuck pilgrim."});
            var turnsCounter = 0;
            while (turnsCounter < turns)
            {
                if (miner.Diggers.TrueForAll(x => !CanDig(x)))
                {

                    gameUi.FastWrite(new[]
                    {
                        "You have no operational diggers.",
                        "The dig cycle is ended."
                    }, ConsoleColor.DarkYellow);
                    turnsCounter = turns;
                    continue;
                }

                gameUi.WriteDigHeader(turnsCounter + 1);
                foreach (var chipDigger in miner.Diggers)
                {
                    if (chipDigger.Durability > 0 && chipDigger.Hopper.Count < chipDigger.Hopper.Max)
                    {
                        var oldDurabiliity = chipDigger.Durability;
                        Console.Write($"{chipDigger.Name}--Digging chips.");
                        Console.CursorVisible = false;
                        var x = 0;
                        var consoleSpinner = new ConsoleSpinner();
                        consoleSpinner.SpinnerAnimationFrames =
                            new[] {">      ", "=>     ", "==>     ", "===>    ", "====>   ", "======> ", "=======>"};
                        while (x < 3)
                        {
                            for (var y = 0; y < 10; y++)
                            {
                                consoleSpinner.UpdateProgress();
                                Thread.Sleep(100);
                            }

                            x++;
                        }

                        Console.CursorVisible = true;
                        var scoop = chipDigger.Dig();
                        var diggerDamage = oldDurabiliity - chipDigger.Durability;
                        chipDigger.Hopper.Count += scoop.Chips;
                        gameUi.ReportScoopResult(chipDigger, diggerDamage, scoop);
                        //Console.WriteLine($"{chipDigger.Name}--{scoop.Chips} dug!");
                        //Console.ForegroundColor = ConsoleColor.DarkRed;
                        //Console.WriteLine($"{chipDigger.Name}--Digger Wear:{diggerDamage}");
                        //if (diggerDamage > 5)
                        //{
                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //    Console.WriteLine("OUCH!");
                        //}

                        //Console.ResetColor();
                        //Console.WriteLine($"{chipDigger.Name}--Digger durability: {chipDigger.Durability}");

                        if (chipDigger.Hopper.IsFull)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine(
                                $"{chipDigger.Name}--The digger hopper is full. Press enter to empty the hopper.");
                            Console.ReadLine();
                            miner.Inventory("chips").Count += chipDigger.Hopper.Count;
                            chipDigger.Hopper.Count = 0;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (chipDigger.Durability < 1)
                        {
                            Console.WriteLine($"{chipDigger.Name}--The digger needs repair!");
                            Console.ResetColor();
                        }
                    }
                }
                turnsCounter++;
            }

            Console.WriteLine("Dig Complete");
            foreach (var chipDigger in miner.Diggers)
            {
                Console.WriteLine(
                    $"Digger Report:{chipDigger.Name} Chips in Hopper:{chipDigger.Hopper.Count}, Durability{chipDigger.Durability}");
            }

            Console.WriteLine($"Chip vault:{miner.Inventory("chips").Count}");
        }
        private static bool CanDig(ChipDigger digger)
        {
            return !digger.Hopper.IsFull && digger.Durability > 0;
        }
    }
}