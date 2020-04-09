using PotatoChipMine.Core.Services.PersistenceService;
using System;

namespace PotatoChipMine.Core.Models
{
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PluralizedName { get; set; }
        public string Description { get; set; } = string.Empty;

        public string GetNameFormBasedOnCount(int quantity)
        {
            return quantity == 1 ? Name : PluralizedName;
        }
    }
}
