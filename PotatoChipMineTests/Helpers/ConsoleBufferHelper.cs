using PotatoChipMine.Core.GameEngine;
using System;
using System.Linq;
using System.Text;

namespace PotatoChipMineTests.Helpers
{
    public static class ConsoleBufferHelper
    {
        public static string GetText(ConsoleBuffer buffer)
        {
            var output = new StringBuilder();

            while (true)
            {
                var c = buffer.Read();
                if (c == null)
                    break;

                output.Append(c.Char);
            }

            var data = output.ToString();
            return data;
        }
    }
}
