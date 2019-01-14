using System.Collections.Generic;
using PotatoChipMine.Services;

namespace PotatoChipMine.ControlRoom.Services
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
        public ControlRoom BuildControlRoom()
        {
            var greeting = new string[]
            {
                "Welcome to digger control.", "From here you can prepare your diggers to dig."
            };
            var controlRoom = new ControlRoom(_ui,_gameState,greeting);
            return controlRoom;
        }
    }
}