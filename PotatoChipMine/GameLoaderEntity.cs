using System.Collections.Generic;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.GameEngine;

namespace PotatoChipMine
{
    public class GameLoaderEntity : GameEntity
    {
        private bool promptUpdated = false;

        public GameLoaderEntity(GameState gameState)
            : base(gameState)
        {
            
        }

        public override void HandleInput(UserCommand command)
        {
            if (command.CommandText.ToLower() == "yes")
            {
                Game.SwitchScene(Scene.Create(new List<IGameEntity>
                {
                    new CollectMineNameEntity(GameState)
                }));
                GameState.PromptText = null;
            }

            if (command.CommandText.ToLower() == "no")
            {
                Game.SwitchScene(Scene.Create(new List<IGameEntity>
                {
                    new LoadGameEntity(GameState, new GamePersistenceService())
                }));
                GameState.PromptText = null;
            }
        }

        public override void Update(Frame frame)
        {
            if (!promptUpdated)
            {
                promptUpdated = true;
                GameState.PromptText = "Do you want to start a new game?";
            }
        }
    }
}