using System;
using System.Collections.Generic;
using System.IO;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.Services.Events;
using PotatoChipMine.GameEngine;
using System.Linq;

namespace PotatoChipMine
{
    public class LoadGameEntity : GameEntity
    {
        private readonly GamePersistenceService persistenceService;
        private bool sentFiles;

        public LoadGameEntity(GameState gameState) : base(gameState)
        {
        }

        public LoadGameEntity(GameState gameState, GamePersistenceService persistenceService)
        : base(gameState)
        {
            this.persistenceService = persistenceService;
        }

        public override void HandleInput(UserCommand command)
        {
            persistenceService.LoadGame(GameState, command.CommandText);
            StartGame();
        }

        public override void Update(Frame frame)
        {
            if (sentFiles)
                return;

            if (Directory.Exists(GameState.SaveDirectory))
            {
                ReportFiles();
            }

            GameState.PromptText = "Enter File Name: ";
            sentFiles = true;
        }

        private void ReportFiles()
        {
            GameState.NewEvents.Add(new GameEvent
            {
                Name = "ListOfFiles",
                Description = "",
                Message = string.Join(
                                Environment.NewLine,
                                persistenceService.SaveFiles(GameState).Select(x => x.Name).ToList()
                                )
            });
        }

        private void StartGame()
        {
            GameState.NewEvents.Add(new GameEvent
            {
                Name = "GetStarted",
                Message = $"Well ok then.  Good luck to you {GameState.Miner.Name}!"
            });

            var initialScene = Scene.Create(new List<IGameEntity>
                {
                    new RestockingEvent(GameState),
                    new LotteryEvent(GameState),
                    new GameRoomManager(GameState)
                });

            GameState.PromptText = null;
            Game.PushScene(initialScene);
        }
    }
}