using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms
{
    public class LobbyRoom : GameRoom
    {
        public LobbyRoom(
            GameUI ui,
            GameState gameState,
            string[] greeting,
            GameMode activeMode,
            CommandsGroup commandGroup)
            : base(ui, gameState, greeting, activeMode)
        {
            this.CommandsGroup = commandGroup;
        }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.Lobby;
            base.EnterRoom();
        }
    }
}