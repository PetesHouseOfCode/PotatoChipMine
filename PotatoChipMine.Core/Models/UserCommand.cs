using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Models
{
    public class UserCommand
    {
        public string CommandText { get; set; }
        public List<string> Parameters { get; set; }
        public string FullCommand { get; set; }
    }
}