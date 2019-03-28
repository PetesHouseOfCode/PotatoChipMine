using PotatoChipMine.GameEngine;
using PotatoChipMine.Models;

namespace PotatoChipMine
{
    internal class GameRoomManager : IGameEntity
    {
        private GameState _gameState;

        public GameRoomManager(GameState gameState)
        {
            _gameState = gameState;
        }

        public void HandleInput(UserCommand command)
        {
            _gameState.CurrentRoom.HandleInput(command);
        }

        public void Update(Frame frame)
        {
            _gameState.CurrentRoom.Update(frame);
        }
    }
}