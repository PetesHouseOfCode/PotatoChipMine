using System.Collections.Generic;

namespace PotatoChipMine.GameEngine
{
    public class ConsoleBuffer 
    {        private Queue<ConsoleChar> Buffer = new Queue<ConsoleChar>();
        
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
    }
}