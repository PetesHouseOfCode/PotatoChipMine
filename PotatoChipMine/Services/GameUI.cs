using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class GameUI
    {
        private const int _defaultTableWidth = 77;
        private int _tableWidth = 77;

        public void Intro()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var s in intro)
            {
                TypeWriterWrite(s,1);
                //Console.WriteLine(s);
            }
            Console.ResetColor();
            Console.WriteLine(Environment.NewLine + AlignCenter("<<< PRESS ENTER >>>",Console.WindowWidth));
            Console.ReadLine();
            Console.Clear();
        }

        public void ReportInfo(string[] linesToReport)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TypeWriterWrite(linesToReport.ToList());
            Console.ResetColor();
        }

        public void FastWrite(string[] linesToReport, ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = color;
            TypeWriterWrite(linesToReport.ToList(), 3);
            Console.ResetColor();
        }

        public void WritePrompt(string linesToReport)
        {
            Console.ForegroundColor = ConsoleColor.White;
            TypeWriterWrite(new List<string>() {linesToReport}, 3);
            Console.ResetColor();
        }

        public UserCommand AcceptUserCommand(string commandContext = "")
        {
            var cmdStr = "Enter Command >>";
            if (commandContext != "")
                cmdStr = $"Enter {commandContext} Command >>";
            WritePrompt(cmdStr);
            var commandEntry = Console.ReadLine()?.Split(' ');
            if (commandEntry == null || commandEntry.Length == 0)
                return new EmptyCommand();
            return new UserCommand {CommandText = commandEntry?[0], Parameters = commandEntry.Skip(1).ToList()};
        }

        public void ReportDiggersStarting(List<ChipDigger> diggers)
        {
            _tableWidth = 50;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            PrintRow("Name","Chip Density", "Site Hardness");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var chipDigger in diggers)
            {
                PrintRow(chipDigger.Name, chipDigger.MineSite.ChipDensity.ToString(),
                    chipDigger.MineSite.Hardness.ToString());
                PrintLine();
            }
            _tableWidth = _defaultTableWidth;
            Console.ResetColor();
        }

        public void ReportVault(GameState gameState)
        {
            FastWrite(new[] {$"Chip Vault: {gameState.Miner.Inventory("chips").Count}"});
            Console.WriteLine();
        }

        public void ReportAvailableCommands(CommandsGroup commandsGroup, GameState gameState)
        {
            FastWrite(new string[]
            {
                $"-----------   {gameState.Mode.ToString().ToUpper()} Commands  ---------------",
                "These commands are available from any game room."
            });
            foreach (var commandsDefinition in commandsGroup.LocalCommands.OrderBy(x => x.Command))
            {
                var command = commandsDefinition.EntryDescription ?? commandsDefinition.Command;
                FastWrite(new[]
                    {$"Command: [{command}]", $"Description: {commandsDefinition.Description}", "--------"});
            }

            if (commandsGroup.ParentGroup == null || !(commandsGroup.ParentGroup.LocalCommands?.Count > 0)) return;

            FastWrite(new[]
                {
                    "-----------   Global Commands  ---------------", "These commands are available from any game room."
                },
                ConsoleColor.Green);
            Console.WriteLine();
            foreach (var globalCommand in commandsGroup.ParentGroup.LocalCommands.OrderBy(x => x.Command))
            {
                var command = globalCommand.EntryDescription ?? globalCommand.Command;
                FastWrite(new[]
                        {$"Command: [{command}]", $"Description: {globalCommand.Description}", "--------"},
                    ConsoleColor.Green);
            }


        }

        public void ReportStoreStock(List<StoreItem> stock, ConsoleColor color = ConsoleColor.Gray)
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            PrintRow(new[] { "Name","Price", "Quantity" });
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (stock.Count < 1) return;
            PrintLine();
            foreach (var storeItem in stock)
            {
                PrintRow(storeItem.Name, storeItem.Price.ToString(), storeItem.Count.ToString());
                PrintLine();
            }
            Console.ResetColor();
         }

        public void ReportBuyingItems(List<StoreItem> buying, ConsoleColor color = ConsoleColor.Gray)
        {
            var list = buying.Select(x => $"Item Name:{x.Name} Price Paid:{x.Price} tt").ToArray();
            FastWrite(list);
        }

        private void TypeWriterWrite(List<string> linesList, int charSpeed = 3)
        {
            foreach (var line in linesList)
            {
                foreach (var x in line)
                {
                    Console.Write(x);
                    Thread.Sleep(charSpeed);
                }

                Console.Write(Environment.NewLine);
                Thread.Sleep(100);
            }
        }

        private void TypeWriterWrite(string line, int charSpeed = 3)
        {
            var linesArry = new List<string>() {line};
            TypeWriterWrite(linesArry, charSpeed);
        }

        public void ReportMinerState(Miner miner)
        {
            var minerState = new List<string>
            {
                $"Name: {miner.Name}",
                $"Chip Vault:{miner.Inventory("chips").Count}",
                $"Tater Tokens:{miner.TaterTokens}",
                $"Diggers Count:{miner.Diggers.Count}"
            };
            FastWrite(minerState.ToArray());
        }

        public void ReportMinerInventory(Miner miner)
        {
            _tableWidth = 77;
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            PrintRow(new[] {"Name", "Quantity"});
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (miner.InventoryItems.Count < 1) return;
            PrintLine();
            foreach (var minerInventoryItem in miner.InventoryItems)
            {
                PrintRow(new[] {minerInventoryItem.Name, minerInventoryItem.Count.ToString()});
                PrintLine();
            }
            Console.ResetColor();
            ResetTableWidth();
        }

        public void ReportDiggerEquipped(string newDiggerName)
        {
            FastWrite(new[] {$"Digger {newDiggerName} is has been equipped"}, ConsoleColor.Yellow);
        }

        public void ReportBadCommand(string badCommand)
        {
            FastWrite(new[] {$"{badCommand} is not a valid command.", "Type [help] to see a list of commands."},
                ConsoleColor.Red);
        }

        public void ReportException(string[] message)
        {
            FastWrite(message, ConsoleColor.Red);
        }

        public void ReportHopperEmptied(string diggerName, int hopperCount, int vaultCount)
        {
            FastWrite(
                new[]
                {
                    $"{hopperCount} was removed from {diggerName}'s hopper and moved into the chip vault.",
                    $"Vault Chips:{vaultCount}"
                }, ConsoleColor.Yellow);
        }

        public void ReportDiggerScrapped(ChipDigger digger, int bolts)
        {
            FastWrite(
                new[]
                {
                    $"{digger} was scrapped for {bolts} bolts."
                }, ConsoleColor.Yellow);

        }

        public bool ConfirmDialog(string[] message)
        {
            ReportInfo(message);
            for (;;)
            {
                var result = Console.ReadLine();

                switch (result)
                {
                    case "yes":
                        return true;
                    case "no":
                        return false;
                    default:
                        FastWrite(new[] {"proceed?"});
                        break;
                }
            }
        }

        private void PrintLine()
        {
            TypeWriterWrite(new string('-', _tableWidth), 1);
        }

        private void PrintRow(params string[] columns)
        {
            var width = (_tableWidth - columns.Length) / columns.Length;
            var row = "|";
            foreach (var column in columns)
            {
                row += AlignCenter(column, width) + "|";
            }

            TypeWriterWrite(row);
        }

        private static string AlignCenter(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        public void ReportDiggers(List<ChipDigger> diggers)
        {
            _tableWidth = 100;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            PrintRow(new[] {"Name", "Durability","Chips in Hopper", "Hopper Size", "Hopper Space"});
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (diggers.Count < 1) return;
            PrintLine();
            foreach (var digger in diggers)
            {
                PrintRow(digger.Name,digger.Durability.ToString(),digger.Hopper.Count.ToString(),digger.Hopper.Max.ToString(),$"{digger.Hopper.Max - digger.Hopper.Count}/{digger.Hopper.Max}");
                PrintLine();
            }
            Console.ResetColor();
            ResetTableWidth();
        }

        private void ResetTableWidth()
        {
            _tableWidth = _defaultTableWidth;
        }

        private string[] intro = new[]
        {
            "        _____________",
            @"      //            \\                   ||                        ||",
            @"      ||             ||                  ||                        ||",
            @"      ||             ||               ////////                  ////////",
            @"      ||             //    ______        ||        _______         ||         ______ ",
            @"      ||____________//    /      \       ||       /       \        ||        /      \    ",
            @"      ||                 |        |      ||      |         |       ||       |        |   ",
            @"      ||                 |        |      ||      |         |\      ||       |        |  ",
            @"      ||                  \______/       ||       \_______/\ \     ||        \______/   ",
            @"",
            @"        _____________",
            @"      //             \\       ||",
            @"      ||              ||      ||                ",
            @"      ||                      ||                \\",
            @"      ||                      ||_________                  _______",
            @"      ||                      ||         \\      ||      //       \\",
            @"      ||                      ||         ||      ||     ||         ||",
            @"      ||              ||      ||         ||      ||     ||         || ",
            @"      \\_____________//       ||         ||      ||     ||_________//",
            @"                                                        ||",
            @"                                                        ||",
            @"      ||\\            //||                              ||",
            @"      || \\          // ||     \\                       ",
            @"      ||  \\        //  ||           \|_________       _______       \\_______"  ,
            @"      ||   \\      //   ||     ||    ||        \\    //       \\     ||      \\",
            @"      ||    \\    //    ||     ||    ||         ||   ||_______//     ||        ",
            @"      ||     \\  //     ||     ||    ||         ||   ||              ||",
            @"      ||      \\//      ||     ||    ||         ||   \\_______//     ||",
            @"",
            @""
        };

        public void WriteDigHeader(int digNumber = 0)
        {
            _tableWidth = 100;
            PrintLine();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            PrintRow($"DIG NUMBER {digNumber}");
            PrintRow("Digger", "Chips Dug", "Damage","Durability","Hopper");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Console.ResetColor();
            _tableWidth = _defaultTableWidth;
        }
        public void ReportScoopResult(ChipDigger chipDigger, int diggerDamage, Scoop scoop)
        {
            _tableWidth = 100;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorLeft=0;
            PrintRow(
                chipDigger.Name, 
                scoop.Chips.ToString(), 
                diggerDamage.ToString(),
                $"{chipDigger.Durability}/{chipDigger.MaxDurability}",
                $"{chipDigger.Hopper.Count}/{chipDigger.Hopper.Max}");
            PrintLine();
            Console.ResetColor();
            _tableWidth = _defaultTableWidth;
        }
    }
}