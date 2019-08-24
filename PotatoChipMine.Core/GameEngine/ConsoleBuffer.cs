using PotatoChipMine.Core.Support;
using System.Collections.Generic;

namespace PotatoChipMine.Core.GameEngine
{
    public class ConsoleBuffer
    {
        private Queue<ConsoleChar> Buffer = new Queue<ConsoleChar>();

        public void Write(ConsoleChar character)
        {
            Buffer.Enqueue(character);
        }

        public ConsoleChar Read()
        {
            // var character = new ConsoleChar('\0');
            Buffer.TryDequeue(out ConsoleChar character);
            return character;
        }

        public IEnumerable<ConsoleChar> Read(int numChars)
        {
            for (int x = 0; x < numChars; x++)
            {
                Buffer.TryDequeue(out ConsoleChar character);
                if (character == null)
                    continue;
                yield return character;
            }
        }
    }
}