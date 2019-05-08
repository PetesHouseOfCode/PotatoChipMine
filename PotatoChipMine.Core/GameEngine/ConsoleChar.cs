using System;

namespace PotatoChipMine.Core.GameEngine
{
    public class ConsoleChar
    {
        public char Char { get; }
        public PcmColor ForegroundColor { get; }
        public PcmColor BackgroundColor { get; }

        public ConsoleChar(char character, PcmColor foregroundColor = null, PcmColor backgroundColor = null)
        {
            Char = character;
            ForegroundColor = foregroundColor ?? PcmColor.White;
            BackgroundColor = backgroundColor ?? PcmColor.Black;
        }
    }
}
