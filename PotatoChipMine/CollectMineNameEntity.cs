using System;
using System.Collections.Generic;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.GameEngine;

namespace PotatoChipMine
{
    public class CollectMineNameEntity : GameEntity
    {
        private bool sentMessage;

        public CollectMineNameEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            if (string.IsNullOrEmpty(command.CommandText))
            {                   
                GameState.NewEvents.Add(new GameEvent
                {
                    Name = "ErrorMessage",
                    Description = "",
                    Message = "Please enter a name."
                });

                return;
            }
         
            GameState.Miner.Name = command.CommandText;
            GameState.PromptText = null;
            Game.SwitchScene(Scene.Create(new List<IGameEntity>{
                new WelcomeEntity(GameState)
            }));
        }

        public override void Update(Frame frame)
        {
            if (!sentMessage)
            {
                GameState.NewEvents.Add(new GameEvent
                {
                    Name = "WelcomeMessage",
                    Description = "",
                    Message = "Howdy pilgrim!  Welcome to glamorous world of 'tater chip mining!" + Environment.NewLine +
                    "I'm Earl, your mine bot. I'll be you're right hand man ... 'er bot, around this here mining operation."
                });

                GameState.PromptText = "Whats your name pilgrim?";
                sentMessage = true;
            }
        }
    }
}