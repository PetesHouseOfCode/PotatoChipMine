using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using PotatoChipMine.GameEngine;
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
            foreach(var line in linesToReport)
                Game.WriteLine(line, color);
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
            if (!showingPrompt)
                return new List<UserCommand>();

            DrawCommandPrompt();

            var commandEntry = GetInput()?.Trim().Split(' ');
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

        public void ShowCommandPrompt()
        {
            if (!showingPrompt)
            {
                Debug.WriteLine("Showing Prompt");
                showingPrompt = true;
                DrawCommandPrompt(true);
            }
        }

        public void HideCommandPrompt()
        {
            if (showingPrompt)
            {
                Debug.WriteLine("Hiding Prompt");
                showingPrompt = false;
                Console.Write($"                                                                                    \r");
            }
        }
    }
}