using PotatoChipMine.Core.Events;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PotatoChipMine.Core.Entities
{
    public class LoadGameEntity : GameEntity
    {
        private readonly GamePersistenceService persistenceService;
        private bool sentFiles;
        private bool sentConfirm;
        private bool confirmed;

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
            if(sentConfirm && GameState.Miner != null)
            {
                if(command.CommandText.ToLower() == "yes")
                {
                    confirmed = true;
                    return;
                }

                if(command.CommandText.ToLower() == "no")
                {
                    Game.PopScene();
                }
            }

            persistenceService.LoadGame(GameState, command.CommandText);
            StartGame();
        }

        public override void Update(Frame frame)
        {
            if (!sentConfirm && GameState.Miner != null)
            {
                sentConfirm = true;
                Game.WriteLine("Loading a new game will end this game and overwrite it with the loaded game data.");
                GameState.PromptText = "Do you wish to proceed? ";
                return;
            }

            if (sentConfirm && GameState.Miner != null && !confirmed)
                return;

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
            Game.WriteLine($"Well ok then.  Good luck to you {GameState.Miner.Name}!", PcmColor.DarkGreen);

            var initialScene = Scene.Create(new List<IGameEntity>
                {
                    new RestockingEvent(GameState),
                    new LotteryEvent(GameState),
                    new DigManagerEntity(GameState),
                    new GameRoomManager(GameState)
                });

            GameState.PromptText = null;
            Game.PushScene(initialScene);
        }
    }
}