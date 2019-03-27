using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class GameUI
    {
        private const int defaultTableWidth = 77;
        private int tableWidth = 77;
        private string incomingCommand = string.Empty;
        private int linePrintSpeed = 1;
        private string oldCommandPrompt = string.Empty;
        private bool showingPrompt = true;

        public GameUI(GameState state)
        {
            this.state = state;
        }

        public void Intro()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var s in intro)
            {
                TypeWriterWrite(AlignCenter(s,Console.WindowWidth), 1);
            }

            Console.ResetColor();
            Console.WriteLine(AlignCenter("<<< PRESS ENTER >>>", Console.WindowWidth));
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

        public void WritePrompt(string commandPrompt, bool force = false, bool showCursor= true)
        {
            if (commandPrompt == oldCommandPrompt && !force)
                return;

            var cursorChar = showCursor ? "\x00A6" : "";
            oldCommandPrompt = commandPrompt;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\r{commandPrompt}{cursorChar} ");
            Console.ResetColor();
        }

        private void DrawCommandPrompt(bool force = false,bool showCursor=true)
        {
            var commandContext = state.PromptText ?? $"{state.CurrentRoom.Name} Command >>" ?? string.Empty;

            if (commandContext != "")
                commandContext += " ";

            var cmdStr = $"{commandContext} {incomingCommand}";
            WritePrompt(cmdStr, force, showCursor);
        }

        public List<UserCommand> AcceptUserCommand()
        {
            DrawCommandPrompt();

            var commandEntry = GetInput()?.Split(' ');
            if (commandEntry == null)
            {
                return new List<UserCommand>();
            }

            Console.WriteLine();
            if (commandEntry.Length == 0)
            {
                return new List<UserCommand>() { new EmptyCommand() };
            }

            return new List<UserCommand>() { new UserCommand { CommandText = commandEntry?[0], Parameters = commandEntry.Skip(1).ToList() } };
        }

        public string AcceptUserDialogInput(string message){
            WritePrompt(message,true);
            var userInput = GetInput()?.Replace(' ','-');
            return userInput;
        }
        public void ReportDiggersStarting(List<ChipDigger> diggers)
        {
            tableWidth = 50;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            PrintRow("Name", "Chip Density", "Site Hardness");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            foreach (var chipDigger in diggers)
            {
                PrintRow(chipDigger.Name, chipDigger.MineSite.ChipDensity.ToString(),
                    chipDigger.MineSite.Hardness.ToString());
                PrintLine();
            }
            tableWidth = defaultTableWidth;
            Console.ResetColor();
        }

        public void ReportVault(GameState gameState)
        {
            FastWrite(new[] { $"Chip Vault: {gameState.Miner.Inventory("chips").Count}" });
            Console.WriteLine();
        }

        internal void ReportHopperIsFull(string diggerName)
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            tableWidth = 100;
            PrintRow($"{diggerName}--The digger hopper is full. Press enter to empty the hopper.");
            Console.CursorLeft = 0;
            Console.ResetColor();
            tableWidth = defaultTableWidth;
        }
        private string GetInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    var returnCommand = incomingCommand;
                    DrawCommandPrompt(true,false);
                    incomingCommand = string.Empty;
                    return returnCommand;
                }

                if (consoleKeyInfo.Key == ConsoleKey.Backspace)
                {
                    if (incomingCommand.Length == 0)
                        return null;

                    incomingCommand = incomingCommand.Substring(0, incomingCommand.Length - 1);
                    return null;
                }

                if (consoleKeyInfo.KeyChar >= '\x0020' && consoleKeyInfo.KeyChar <= '\x007e')
                    incomingCommand += consoleKeyInfo.KeyChar;

            }

            return null;
        }

        public void ReportAvailableCommands(GameState gameState)
        {
            FastWrite(new string[]
            {
                $"-----------   {gameState.Mode.ToString().ToUpper()} Commands  ---------------"
            });
            foreach (var commandsDefinition in gameState.CurrentRoom.CommandsGroup.LocalCommands.OrderBy(x => x.Command))
            {
                var command = commandsDefinition.EntryDescription ?? commandsDefinition.Command;
                FastWrite(new[]
                    {$"Command: [{command}]", $"Description: {commandsDefinition.Description}", "--------"});
            }
        }

        public void ReportStoreStock(List<StoreItem> stock, ConsoleColor color = ConsoleColor.Gray)
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            PrintRow(new[] { "Name", "Price", "Quantity" });
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
                    if (x != ' ')
                        Thread.Sleep(charSpeed);
                }

                Console.Write(Environment.NewLine);
                Thread.Sleep(linePrintSpeed);
            }
        }

        private void TypeWriterWrite(string line, int charSpeed = 3)
        {
            var linesArray = new List<string>() { line };
            TypeWriterWrite(linesArray, charSpeed);
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
            tableWidth = 77;
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            PrintRow(new[] { "Name", "Quantity" });
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (miner.InventoryItems.Count < 1) return;
            PrintLine();
            foreach (var minerInventoryItem in miner.InventoryItems)
            {
                PrintRow(new[] { minerInventoryItem.Name, minerInventoryItem.Count.ToString() });
                PrintLine();
            }
            Console.ResetColor();
            ResetTableWidth();
        }

        public void ReportDiggerEquipped(string newDiggerName)
        {
            FastWrite(new[] { $"Digger {newDiggerName} is has been equipped" }, ConsoleColor.Yellow);
        }

        public void ReportBadCommand(string badCommand)
        {
            FastWrite(new[] { $"{badCommand} is not a valid command.", "Type [help] to see a list of commands." },
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
                    $"{digger.Name} was scrapped for {bolts} bolts."
                }, ConsoleColor.Yellow);

        }

        public bool ConfirmDialog(string[] message)
        {
            ReportInfo(message);
            for (; ; )
            {
                var result = Console.ReadLine();

                switch (result.ToLower())
                {
                    case "yes":
                        return true;
                    case "no":
                    case "cancel":
                        return false;
                    default:
                        ReportInfo(message);
                        break;
                }
            }
        }

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
            tableWidth = 100;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            PrintRow(new[] { "Name", "Durability", "Chips in Hopper", "Hopper Size", "Hopper Space" });
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (diggers.Count < 1) return;
            PrintLine();
            foreach (var digger in diggers)
            {
                PrintRow(digger.Name, digger.Durability.ToString(), digger.Hopper.Count.ToString(), digger.Hopper.Max.ToString(), $"{digger.Hopper.Max - digger.Hopper.Count}/{digger.Hopper.Max}");
                PrintLine();
            }
            Console.ResetColor();
            ResetTableWidth();
        }

        private void ResetTableWidth()
        {
            tableWidth = defaultTableWidth;
        }

        private readonly string[] intro = new[]
        {
            "╔════════════════════════════════════════════════════════════════════════════════════════╗",
            "║       _____________                                                                    ║",
            @"║      //            \\                   ||                        ||                   ║",
            @"║      ||             ||                  ||                        ||                   ║",
            @"║      ||             ||               ////////                  ////////                ║",
            @"║      ||             //    ______        ||        _______         ||         ______    ║",
            @"║      ||____________//    /      \       ||       /       \        ||        /      \   ║",
            @"║      ||                 |        |      ||      |         |       ||       |        |  ║",
            @"║      ||                 |        |      ||      |         |\      ||       |        |  ║",
            @"║      ||                  \______/       ||       \_______/\ \     ||        \______/   ║",
            @"║                                                                                        ║",
            @"║        _____________                                                                   ║",
            @"║      //             \\       ||                                                        ║",
            @"║      ||              ||      ||                                                        ║",
            @"║      ||                      ||                \\                                      ║",
            @"║      ||                      ||_________                  _______                      ║",
            @"║      ||                      ||         \\      ||      //       \\                    ║",
            @"║      ||                      ||         ||      ||     ||         ||                   ║",
            @"║      ||              ||      ||         ||      ||     ||         ||                   ║",
            @"║      \\_____________//       ||         ||      ||     ||_________//                   ║",
            @"║                                                        ||                              ║",
            @"║                                                        ||                              ║",
            @"║      ||\\            //||                              ||                              ║",
            @"║      || \\          // ||     \\                                                       ║",
            @"║      ||  \\        //  ||           \|_________       _______       \\_______          ║"  ,
            @"║      ||   \\      //   ||     ||    ||        \\    //       \\     ||      \\         ║",
            @"║      ||    \\    //    ||     ||    ||         ||   ||_______//     ||                 ║",
            @"║      ||     \\  //     ||     ||    ||         ||   ||              ||                 ║",
            @"║      ||      \\//      ||     ||    ||         ||   \\_______//     ||                 ║",
            @"║                                                                                        ║",
            @"╚════════════════════════════════════════════════════════════════════════════════════════╝"
        };
        private readonly GameState state;

        public void WriteDigHeader(int digNumber = 0)
        {
            tableWidth = 100;
            PrintLine();
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            PrintRow($"DIG NUMBER {digNumber}");
            PrintRow("Digger", "Chips Dug", "Damage", "Durability", "Hopper");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintLine();
            Console.ResetColor();
            tableWidth = defaultTableWidth;
        }
        public void ReportScoopResult(ChipDigger chipDigger, int diggerDamage, Scoop scoop)
        {
            tableWidth = 100;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.CursorLeft = 0;
            PrintRow(
                chipDigger.Name,
                scoop.Chips.ToString(),
                diggerDamage.ToString(),
                $"{chipDigger.Durability}/{chipDigger.MaxDurability}",
                $"{chipDigger.Hopper.Count}/{chipDigger.Hopper.Max}");
            PrintLine();
            Console.ResetColor();
            tableWidth = defaultTableWidth;
        }

        public void ReportDiggerNeedsRepair(ChipDigger chipDigger)
        {
            tableWidth = 100;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            PrintRow($"{chipDigger.Name}--The digger needs repair!");
            Console.ResetColor();
            tableWidth = defaultTableWidth;
        }

        public void ReportSaveGames(FileInfo[] getFiles)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PrintLine();
            PrintRow("Game saves");
            PrintLine();
            PrintRow("Save Name", "Saved Date");
            PrintLine();
            foreach (var file in getFiles)
            {
                var saveName = Path.GetFileName(file.Name).Split(".")[0];
                var updated = file.LastWriteTime.ToString();
                PrintRow(saveName, updated);
                PrintLine();
            }
            Console.ResetColor();
        }

        public string CollectGameSaveToLoad(FileInfo[] files)
        {
            ReportSaveGames(files);
            FastWrite(new[] { "Enter the name of the saved game you'd like to Resume" });
            return Console.ReadLine() ?? string.Empty;
        }

        public string SavePrompt(bool newSave)
        {
            var promptMessage = new List<string>
            {
                "Enter the name you would like to use to save this game.",
                "Type [cancel] to cancel the save operation."
            };
            if (newSave)
            {
                promptMessage.Insert(0, "You have not previously saved this game.");
            }
            string saveName;
            do
            {
                FastWrite(promptMessage.ToArray());
                saveName = Console.ReadLine() ?? string.Empty;
                if (saveName.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    ReportException(new[] { "Save cancelled." });
                    return string.Empty;
                }
            } while (saveName == string.Empty);
            return saveName;
        }

        public void ReportEvent(string message)
        {
            HideCommandPrompt();
            Console.ForegroundColor = ConsoleColor.Magenta;
            PrintLine();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Magenta;
            TypeWriterWrite(new List<string> { AlignCenter($"*** {message} ****", Console.WindowWidth) });
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Magenta;
            PrintLine();
            Console.ResetColor();
            ShowCommandPrompt();
        }

        public void ShowCommandPrompt()
        {
            if (!showingPrompt)
            {
                showingPrompt = true;
                DrawCommandPrompt(true);
            }
        }

        public void HideCommandPrompt()
        {
            if (showingPrompt)
            {
                showingPrompt = false;
                Console.Write($"                                                                                    \r");
            }
        }
    }
}