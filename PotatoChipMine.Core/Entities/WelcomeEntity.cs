using PotatoChipMine.Core.Events;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Entities
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
                Game.ClearConsole();
                var message = "I'm sorry to report that the tutorial is under construction, but here's what we've got so far..." + Environment.NewLine + Environment.NewLine +
                        "** You can type [help] at any time to see a list of available commands." + Environment.NewLine + Environment.NewLine +
                        "** You can buy and sell things in the store." + Environment.NewLine +
                        "** Type [store] to enter the store." + Environment.NewLine + Environment.NewLine +
                        "** You can take actions related to your diggers in the control-room." + Environment.NewLine +
                        "** Type [control-room] to enter the control-room" + Environment.NewLine + Environment.NewLine;

                Game.WriteLine(message, PcmColor.Blue);

                StartGame();
            }
        }

        private void StartGame()
        {
            Game.WriteLine($"Well ok then.  Good luck to you {GameState.Miner.Name}!", PcmColor.Blue);

            var initialScene = Scene.Create(new List<IGameEntity>
                {
                    new RestockingEvent(GameState),
                    new LotteryEvent(GameState),
                    new DigManagerEntity(GameState),
                    new GameRoomManager(GameState),
                    new MinerAchievementsMonitorEntity(GameState)
                });

            GameState.PromptText = null;
            Game.PushScene(initialScene);
        }

        public override void Update(Frame frame)
        {
            if (sentMessage)
                return;

            Game.ClearConsole();
            Game.WriteLine($"Very pleased to meet you {GameState.Miner.Name}." + Environment.NewLine +
                           "If you're new to 'tater mining you may want some instructions..." + Environment.NewLine +
                           "You look like maybe you know your way around a chip digger though." + Environment.NewLine +
                           "To learn more about playing the game, type [yes].  If you're ready to start mining some chips type [no].");
            GameState.PromptText = "Do you need instructions?";
            sentMessage = true;
        }
    }
}