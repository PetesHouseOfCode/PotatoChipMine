using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms
{
    public class LobbyRoom : GameRoom
    {
        public LobbyRoom(GameUI ui, GameState gameState, string[] greeting, GameMode activeMode)
            : base(ui, gameState, greeting, activeMode)
        {
        }
    }
}