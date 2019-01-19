using PotatoChipMine.GameRooms.ControlRoom.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom
{
    public class DiggerControlRoom : GameRoom
    {
        protected new readonly CommandsGroup CommandsGroup;

        public DiggerControlRoom(GameUI ui, GameState gameState, string[] greeting) :
            base(ui, gameState, greeting, GameMode.ControlRoom)
        {
            CommandsGroup = new ControlRoomCommandsGroupFactory(ui).Build();
        }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.ControlRoom;
            base.EnterRoom();
        }

        protected override void AcceptCommand()
        {
            var command = Ui.AcceptUserCommand("control-room");
            CommandsGroup.ExecuteCommand(Ui, command, GameState);
        }
    }
}