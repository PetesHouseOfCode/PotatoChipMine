using System;
using System.Collections.Generic;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.GameEngine;

namespace PotatoChipMine.Entities
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
                Game.WriteLine("Please enter a name.", ConsoleColor.Red);

                return;
            }
         
            GameState.Miner = Miner.Default();
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
                Game.WriteLine("Howdy pilgrim!  Welcome to glamorous world of 'tater chip mining!" + Environment.NewLine +
                    "I'm Earl, your mine bot. I'll be you're right hand man ... 'er bot, around this here mining operation.");
                GameState.PromptText = "Whats your name pilgrim?";
                sentMessage = true;
            }
        }
    }
}