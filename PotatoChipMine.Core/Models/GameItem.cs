using PotatoChipMine.Core.Services.PersistenceService;
using System;

namespace PotatoChipMine.Core.Models
{
    public class GameItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        public virtual GameItemState GetState()
        {
            return new GameItemState
            {
                ItemId = ItemId,
                Name = Name,
                Description = Description,
                Type = "Base"
            };
        }
        public static GameItem From(GameItemState state)
        {
            return new GameItem
            {
                ItemId = state.ItemId,
                Name = state.Name,
                Description = state.Description
            };
        }
    }
}