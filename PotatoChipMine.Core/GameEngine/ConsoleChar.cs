using System;

namespace PotatoChipMine.Core.GameEngine
{
    public class ConsoleChar
    {
        public char Char { get; }

        public ConsoleColor ForegroundColor { get; }
        public ConsoleColor BackgroundColor { get; }

        public ConsoleChar(char character, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Char = character;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
