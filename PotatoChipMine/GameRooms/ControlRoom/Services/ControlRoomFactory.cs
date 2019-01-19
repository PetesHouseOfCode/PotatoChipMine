using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom.Services
{
    public class ControlRoomFactory
    {
        private readonly GameUI _ui;
        private readonly GameState _gameState;

        public ControlRoomFactory(GameUI ui,GameState gameState)
        {
            _gameState = gameState;
            _ui = ui;
        }
        public DiggerControlRoom BuildControlRoom()
        {
            var greeting = new string[]
            {
                "Welcome to digger control.", "From here you can prepare your diggers to dig."
            };
            var controlRoom = new DiggerControlRoom(_ui,_gameState,greeting);
            return controlRoom;
        }
    }
}