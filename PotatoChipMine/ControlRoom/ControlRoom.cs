using PotatoChipMine.ControlRoom.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.ControlRoom
{
    public class ControlRoom : GameRoom
    {
        protected new readonly CommandsGroup CommandsGroup;

        public ControlRoom(GameUI ui, GameState gameState, string[] greeting) :
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
            var command = Ui.AcceptUserCommand("ControlRoom");
            CommandsGroup.ExecuteCommand(Ui, command, GameState);
        }
    }
}