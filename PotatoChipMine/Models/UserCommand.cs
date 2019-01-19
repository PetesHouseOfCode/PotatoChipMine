using System.Collections.Generic;

namespace PotatoChipMine.Models
{
    public class UserCommand
    {
        public string CommandText { get; set; }
        public List<string> Parameters { get; set; }
    }
}