using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;

namespace PotatoChipMine.Core.GameRooms.ControlRoom.Services
{
    public class ControlRoomFactory
    {
        private readonly GameState _gameState;
        private readonly CommandsGroup _baseCommandsGroup;

        public ControlRoomFactory(
            GameState gameState,
            CommandsGroup baseCommandsGroup)
        {
            _gameState = gameState;
            _baseCommandsGroup = baseCommandsGroup;
        }
        public DiggerControlRoom BuildControlRoom()
        {
            var greeting = new string[]
            {
                "Welcome to digger control.", "From here you can prepare your diggers to dig."
            };
            var controlRoom = new DiggerControlRoom(
                _gameState,
                greeting,
                _baseCommandsGroup.Join(new ControlRoomCommandsGroupFactory().Build())
                );
            return controlRoom;
        }
    }
}