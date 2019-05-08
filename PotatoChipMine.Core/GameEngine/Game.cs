using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameEngine
{
    public static class Game
    {
        private static IPotatoChipGame mainProcess;
        public static void SetMainProcess(IPotatoChipGame mainProcess)
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

        public static void WriteLine(string text, PcmColor color = null, PcmColor backgroundColor = null)
        {
            if (!text.EndsWith(Environment.NewLine))
                text = text + Environment.NewLine;

            foreach (var c in text)
            {
                mainProcess.Output.Write(new ConsoleChar(c, color ?? PcmColor.White, backgroundColor ?? PcmColor.Black));
            }
        }

        public static void Write(string text, PcmColor color = null, PcmColor backgroundColor = null)
        {
            foreach (var c in text)
            {
                mainProcess.Output.Write(new ConsoleChar(c, color ?? PcmColor.White, backgroundColor ?? PcmColor.Black));
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

        private static void PrintLine(int width, PcmColor color, PcmColor backgroundColor)
        {
            WriteLine(new string('-', width), color, backgroundColor);
        }

        private static void PrintRow(int width, PcmColor color, PcmColor backgroundColor, params string[] columns)
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