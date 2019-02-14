using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom.Services
{
    public class ControlRoomFactory
    {
        private readonly GameUI _ui;
        private readonly GameState _gameState;
        private readonly CommandsGroup _baseCommandsGroup;

        public ControlRoomFactory(
            GameUI ui,
            GameState gameState,
            CommandsGroup baseCommandsGroup)
        {
            _gameState = gameState;
            _baseCommandsGroup = baseCommandsGroup;
            _ui = ui;
        }
        public DiggerControlRoom BuildControlRoom()
        {
            var greeting = new string[]
            {
                "Welcome to digger control.", "From here you can prepare your diggers to dig."
            };
            var controlRoom = new DiggerControlRoom(
                _ui,
                _gameState,
                greeting,
                _baseCommandsGroup.Join(new ControlRoomCommandsGroupFactory(_ui).Build())
                );
            return controlRoom;
        }
    }
}