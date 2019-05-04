using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameRooms
{
    public class LobbyRoom : GameRoom
    {
        public LobbyRoom(
            GameState gameState,
            string[] greeting,
            GameMode activeMode,
            CommandsGroup commandGroup)
            : base(gameState, greeting, activeMode)
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