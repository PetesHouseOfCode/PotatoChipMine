using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.GameEngine
{
    public static class Game
    {
        private static MainProcess mainProcess;
        public static void SetMainProcess(MainProcess mainProcess)
        {
            Game.mainProcess = mainProcess;
        }

        public static void SwitchScene(Scene scene)
        {
            mainProcess.CurrentScene = scene;
        }

        public static void PushScene(Scene scene)
        {
            mainProcess.SceneStack.Push(mainProcess.CurrentScene);
            mainProcess.CurrentScene = scene;
        }

        public static void PopScene()
        {
            mainProcess.CurrentScene = mainProcess.SceneStack.Pop();
        }

        public static void Write(ConsoleChar character)
        {
            mainProcess.Output.Write(character);
        }

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            if (!text.EndsWith(Environment.NewLine))
                text = text + Environment.NewLine;

            foreach (var c in text)
            {
                mainProcess.Output.Write(new ConsoleChar(c, color, backgroundColor));
            }
        }

        public static void Write(string text, ConsoleColor color = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            foreach (var c in text)
            {
                mainProcess.Output.Write(new ConsoleChar(c, color, backgroundColor));
            }
        }

        public static void Write(TableOutput table)
        {
            PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor);
            PrintRow(table.Width, table.BackgroundColor, table.ForegroundColor, table.Header.ToArray());
            PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor);
            if (!table.Rows.Any())
                PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor);

            foreach (var row in table.Rows)
            {
                PrintRow(table.Width, table.ForegroundColor, table.BackgroundColor, row.ToArray());
                PrintLine(table.Width, table.ForegroundColor, table.BackgroundColor);
            }
        }

        private static void PrintLine(int width, ConsoleColor color, ConsoleColor backgroundColor)
        {
            WriteLine(new string('-', width), color, backgroundColor);
        }

        private static void PrintRow(int width, ConsoleColor color, ConsoleColor backgroundColor, params string[] columns)
        {
            var columnWidth = (width - columns.Length) / columns.Length;
            var row = "|";
            foreach (var column in columns)
            {
                row += AlignCenter(column, columnWidth) + "|";
            }

            WriteLine(row, color, backgroundColor);
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

        public TableOutput(int width, ConsoleColor foregroundColor = ConsoleColor.Cyan, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            this.width = width;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public int Width => width;

        public int Columns { get; private set; }

        public List<string> Header { get; set; } = new List<string>();

        public List<List<string>> Rows { get; set; } = new List<List<string>>();
        public ConsoleColor ForegroundColor { get; }
        public ConsoleColor BackgroundColor { get; }

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