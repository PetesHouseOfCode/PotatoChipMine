using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameEngine
{
    public static class Game
    {
        private static IPotatoChipGame potatoChipGame;

        public static List<GameAchievement> Achievements { get; set; }

        public static void SetMainProcess(IPotatoChipGame mainProcess)
        {
            Game.potatoChipGame = mainProcess;
        }

        public static void SwitchScene(Scene scene)
        {
            potatoChipGame.CurrentScene = scene;
        }

        public static void PushScene(Scene scene)
        {
            potatoChipGame.SceneStack.Push(potatoChipGame.CurrentScene);
            potatoChipGame.CurrentScene = scene;
        }

        public static void PopScene()
        {
            potatoChipGame.CurrentScene = potatoChipGame.SceneStack.Pop();
        }

        public static void Write(ConsoleChar character, GameConsoles targetConsole = GameConsoles.Output)
        {
            ConsoleBuffer consoleBuffer;
            switch (targetConsole)
            {
                case GameConsoles.Events:
                    consoleBuffer = potatoChipGame.Events;
                    break;
                default:
                    consoleBuffer = potatoChipGame.Output;
                    break;
            }

            consoleBuffer.Write(character);
        }

        public static void WriteLine(string text, PcmColor color = null, PcmColor backgroundColor = null,GameConsoles targetConsole = GameConsoles.Output)
        {
            ConsoleBuffer consoleBuffer;
            switch (targetConsole)
            {

                case GameConsoles.Events:
                    consoleBuffer = potatoChipGame.Events;
                    break;
                default:
                    consoleBuffer = potatoChipGame.Output;
                    break;
            }
            if (!text.EndsWith(Environment.NewLine))
                text += Environment.NewLine;

            foreach (var c in text)
            {
                consoleBuffer.Write(new ConsoleChar(c, color ?? PcmColor.White, backgroundColor ?? PcmColor.Black));
            }
        }

        public static void Write(string text, PcmColor color = null, PcmColor backgroundColor = null,GameConsoles targetConsole = GameConsoles.Output)
        {
            ConsoleBuffer consoleBuffer;
            switch (targetConsole)
            {

                case GameConsoles.Events:
                    consoleBuffer = potatoChipGame.Events;
                    break;
                default:
                    consoleBuffer = potatoChipGame.Output;
                    break;
            }
            foreach (var c in text)
            {
                consoleBuffer.Write(new ConsoleChar(c, color ?? PcmColor.White, backgroundColor ?? PcmColor.Black));
            }
        }

        public static void Write(TableOutput table, GameConsoles targetConsole = GameConsoles.Output)
        {
            PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor, targetConsole);
            PrintRow(table.Width, table.BackgroundColor, table.ForegroundColor,targetConsole, table.Header.ToArray());
            PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor, targetConsole);
            if (!table.Rows.Any())
                PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor, targetConsole);

            foreach (var row in table.Rows)
            {
                PrintRow(table.Width, table.ForegroundColor, table.BackgroundColor,targetConsole, row.ToArray());
                PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor, targetConsole);
            }
        }

        public static void ClearConsole(GameConsoles targetConsole = GameConsoles.Output)
        {
            potatoChipGame.ClearConsole(targetConsole);
        }

        private static void PrintLine(int width, PcmColor color, PcmColor backgroundColor, GameConsoles targetConsole = GameConsoles.Output)
        {
            WriteLine(new string('-', width), color, backgroundColor,targetConsole);
        }

        private static void PrintRow(int width, PcmColor color, PcmColor backgroundColor, GameConsoles targetConsole = GameConsoles.Output, params string[] columns)
        {
            var columnWidth = (width - columns.Length) / columns.Length;
            var row = "|";
            foreach (var column in columns)
            {
                row += AlignCenter(column, columnWidth) + "|";
            }

            WriteLine(row, color, backgroundColor,targetConsole);
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
    }

    public class TableOutput
    {
        private readonly int width;

        public TableOutput(int width, PcmColor foregroundColor = null, PcmColor backgroundColor = null)
        {
            this.width = width;
            ForegroundColor = foregroundColor ?? PcmColor.Cyan;
            BackgroundColor = backgroundColor ?? PcmColor.Black;
        }

        public int Width => width;

        public int Columns { get; private set; }

        public List<string> Header { get; set; } = new List<string>();

        public List<List<string>> Rows { get; set; } = new List<List<string>>();
        public PcmColor ForegroundColor { get; }
        public PcmColor BackgroundColor { get; }

        public void AddHeaders(params string[] headers)
        {
            if (headers.Length > Columns)
                Columns = headers.Length;

            Header = headers.ToList();
        }

        public void AddRow(params string[] data)
        {
            if (data.Length > Columns)
                Columns = data.Length;

            Rows.Add(data.ToList());
        }
    }
}