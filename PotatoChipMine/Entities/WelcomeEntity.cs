using System;
using System.Collections.Generic;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.Services.Events;
using PotatoChipMine.GameEngine;

namespace PotatoChipMine.Entities
{
    public class WelcomeEntity : GameEntity
    {
        private bool sentMessage;

        public WelcomeEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            if (command.CommandText.ToLower() == "no")
            {
                StartGame();
            }

            if (command.CommandText.ToLower() == "yes")
            {
                var message ="I'm sorry to report that the tutorial is under construction, but here's what we've got so far..." + Environment.NewLine + Environment.NewLine +
                        "** You can type [help] at any time to see a list of available commands." + Environment.NewLine + Environment.NewLine +
                        "** You can buy and sell things in the store." + Environment.NewLine +
                        "** Type [store] to enter the store." + Environment.NewLine  + Environment.NewLine +
                        "** You can take actions related to your diggers in the control-room." + Environment.NewLine +
                        "** Type [control-room] to enter the control-room"  + Environment.NewLine  + Environment.NewLine;

                Game.WriteLine(message, ConsoleColor.Blue);

                StartGame();          
            }
        }

        private void StartGame()
        {
            Game.WriteLine($"Well ok then.  Good luck to you {GameState.Miner.Name}!", ConsoleColor.Blue);
            
            var initialScene = Scene.Create(new List<IGameEntity>
                {
                    new RestockingEvent(GameState),
                    new LotteryEvent(GameState),
                    new GameRoomManager(GameState)
                });

            GameState.PromptText = null;
            Game.PushScene(initialScene);
        }

        public override void Update(Frame frame)
        {
            if (sentMessage)
                return;
            
            GameState.NewEvents.Add(new GameEvent
            {
                Name = "WelcomeMessage",
                Description = "",
                Message =$"Very pleased to meet you {GameState.Miner.Name}." + Environment.NewLine +
                         "If you're new to 'tater mining you may want some instructions..." + Environment.NewLine +
                         "You look like maybe you know your way around a chip digger though."
            });

            GameState.PromptText = "Do you need instructions?";
            sentMessage = true;
        }
    }
}