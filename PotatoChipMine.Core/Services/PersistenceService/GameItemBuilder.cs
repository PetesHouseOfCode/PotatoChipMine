using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public static class GameItemBuilder
    {
        public static GameItem Build(GameItemState state)
        {
            switch (state.Type)
            {
                case GameItemStateTypes.Base:
                    return GameItem.From(state);
                case GameItemStateTypes.ChipsHopperUpgradeItem:
                    return ChipsHopperUpgradeItem.From(state);
                case GameItemStateTypes.BitUpgradeItem:
                    return BitUpgradeItem.From(state);
            }

            return GameItem.From(state);
        }
    }
}
