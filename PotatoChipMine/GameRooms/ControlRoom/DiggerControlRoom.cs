using PotatoChipMine.GameRooms.ControlRoom.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom
{
    public class DiggerControlRoom : GameRoom
    {
        public DiggerControlRoom(
            GameUI ui,
            GameState gameState,
            string[] greeting,
            CommandsGroup commandsGroup)
         : base(ui, gameState, greeting, GameMode.ControlRoom)
        {
            this.Name = "control-room";
            CommandsGroup = commandsGroup;
        }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.ControlRoom;
            base.EnterRoom();
        }
    }
}