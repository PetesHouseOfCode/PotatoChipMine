using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class GameItemState
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> MetaData { get; set; } = new Dictionary<string, string>();
    }
}
