using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Services
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
