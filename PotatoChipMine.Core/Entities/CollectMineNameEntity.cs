using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Entities
{
    public class CollectMinerNameEntity : GameEntity
    {
        private bool sentMessage;

        public CollectMinerNameEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            if (string.IsNullOrEmpty(command.FullCommand))
            {
                Game.WriteLine("Please enter a name.", PcmColor.Red);

                return;
            }

            GameState.Miner = Miner.Default();
            GameState.Miner.Name = command.FullCommand;
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
                Game.WriteLine("Whats your name pilgrim?");
                GameState.PromptText = "Enter your name:";
                sentMessage = true;
            }
        }
    }
}