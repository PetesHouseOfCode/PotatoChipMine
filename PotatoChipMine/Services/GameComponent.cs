using System;
using PotatoChipMine.GameEngine;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public abstract class GameComponent : IGameComponent
    {
        protected GameState GameState { get; set; }

        public GameComponent(GameState gameState)
        {
            this.GameState = gameState;
        }

        public virtual void Update(Frame frame)
        {
            throw new NotImplementedException();
        }
    }
}
