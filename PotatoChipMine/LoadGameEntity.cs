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
            var table = new TableOutput(77);
            table.AddHeaders("File Name", "Save Date");
            foreach( var file in persistenceService.SaveFiles(GameState))
            {
                table.AddRow(file.Name, file.LastWriteTime.ToShortDateString());
            }

            Game.Write(table);
        }

        private void StartGame()
        {
            Game.WriteLine($"Well ok then.  Good luck to you {GameState.Miner.Name}!", ConsoleColor.DarkGreen);

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