using PotatoChipMine.Core.Services.PersistenceService;
using System;

namespace PotatoChipMine.Core.Models
{
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        public virtual GameItemState GetState()
        {
            return new GameItemState
            {
                ItemId = Id,
                Name = Name,
                Description = Description,
                Type = "Base"
            };
        }
        public static GameItem From(GameItemState state)
        {
            return new GameItem
            {
                Id = state.ItemId,
                Name = state.Name,
                Description = state.Description
            };
        }
    }
}