using System;
using PotatoChipMine.GameEngine;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public abstract class GameEntity : IGameEntity
    {
        protected GameState GameState { get; set; }

        public GameEntity(GameState gameState)
        {
            this.GameState = gameState;
        }

        public virtual void Update(Frame frame)
        {
        }

        public virtual void HandleInput(UserCommand command)
        {
        }
    }
}
