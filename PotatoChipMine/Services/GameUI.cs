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
        public void Intro()
        {
            foreach (var s in intro)
            {
                Console.WriteLine(s);
            }

            for (var x = 0; x < 10; x++)
            {
                Console.WriteLine();
                Thread.Sleep(30);
            }
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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var chipDigger in diggers)
            {
                TypeWriterWrite(new List<string>
                {
                    $"{chipDigger.Name}",
                    $"---Mine site density:{chipDigger.MineSite.ChipDensity}.",
                    $"---Site Hardness:{chipDigger.MineSite.Hardness}"
                });
                Console.WriteLine("");
            }

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
            var inv = stock
                .Select(x => $"ID:{x.InventoryId} Item Name:{x.Name} Item Price:{x.Price} Quantity:{x.Count}")
                .ToArray();
            FastWrite(inv);
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
            if (miner.Diggers.Count > 0)
                minerState.Add("----------Digger Detail-----------");
            foreach (var minerDigger in miner.Diggers)
            {
                minerState.Add(
                    $"Name:{minerDigger.Name} Durability:{minerDigger.Durability} Chips In Hopper:{minerDigger.Hopper.Count}/{minerDigger.Hopper.Max}");
            }

            FastWrite(minerState.ToArray());
        }

        public void ReportMinerInventory(Miner miner)
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.White;
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

        static int tableWidth = 77;

        private void PrintLine()
        {
            TypeWriterWrite(new string('-', tableWidth), 1);
        }

        private void PrintRow(params string[] columns)
        {
            var width = (tableWidth - columns.Length) / columns.Length;
            var row = "|";
            foreach (var column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            TypeWriterWrite(row);
        }

        static string AlignCentre(string text, int width)
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

        private string[] intro = new[]
        {
            "  _____________",
            @"//------------\\                   ||                        ||",
            @"||             ||                  ||                        ||",
            @"||             ||               ////////                  ////////",
            @"||             //    ______        ||        _______         ||         ______ ",
            @"||____________//    /      \       ||       /       \        ||        /      \    ",
            @"||                 |        |      ||      |         |       ||       |        |   ",
            @"||                 |        |      ||      |         |\      ||       |        |  ",
            @"||                  \______/       ||       \_______/\ \     ||        \______/   ",
            @"",
            @"  _____________",
            @"//             \\       ||",
            @"||              ||      ||                ",
            @"||                      ||                \\",
            @"||                      ||_________                  _______",
            @"||                      ||         \\      ||      //       \\",
            @"||                      ||         ||      ||     ||         ||",
            @"||              ||      ||         ||      ||     ||         || ",
            @"\\_____________//       ||         ||      ||     ||_________//",
            @"                                                  ||",
            @"||\\            //||                              ||",
            @"|| \\          // ||     \\                       ||",
            @"||  \\        //  ||           \|_________       _______",
            @"||   \\      //   ||     ||    ||        \\    //       \\",
            @"||    \\    //    ||     ||    ||         ||   ||_______//",
            @"||     \\  //     ||     ||    ||         ||   ||",
            @"||      \\//      ||     ||    ||         ||    \\______//"
        };
    }
}