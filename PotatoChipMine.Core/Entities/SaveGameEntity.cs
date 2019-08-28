using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services.PersistenceService;
using System;

namespace PotatoChipMine.Core.Entities
{
    public class SaveGameEntity : GameEntity
    {
        private readonly GamePersistenceService persistenceService;

        private bool requestedFilename;
        private bool sentOverwritePrompt;
        private bool doNotOverwrite;
        private bool IsNewSaveFile => GameState.SaveName == string.Empty;

        public SaveGameEntity(GameState gameState)
            : base(gameState)
        {
        }
        public SaveGameEntity(GameState gameState, GamePersistenceService persistenceService)
            : base(gameState)
        {
            this.persistenceService = persistenceService;
        }


        public override void HandleInput(UserCommand command)
        {
            if (!IsNewSaveFile && sentOverwritePrompt)
            {
                if (command.CommandText.ToLower() == "yes")
                {
                    SaveGame();
                    return;
                }

                if (command.CommandText.ToLower() == "no")
                {
                    doNotOverwrite = true;
                    return;
                }
            }

            if ((IsNewSaveFile || doNotOverwrite) && requestedFilename)
            {
                if (string.IsNullOrEmpty(command.FullCommand))
                {
                    Game.WriteLine("File name or cancel is required!", PcmColor.Red);
                    return;
                }

                if (command.FullCommand.ToLower() == "cancel")
                {
                    Game.WriteLine("Game save was cancelled!", PcmColor.Red);
                    GameState.PromptText = null;
                    Game.PopScene();
                    return;
                }

                GameState.SaveName = command.FullCommand;
                SaveGame();
            }
        }

        private void SaveGame()
        {
            persistenceService.SaveGame(GameState);
            GameState.PromptText = null;
            Game.PopScene();
        }

        public override void Update(Frame frame)
        {
            if ((IsNewSaveFile || doNotOverwrite) && !requestedFilename)
            {
                requestedFilename = true;

                Game.WriteLine("Enter the name you would like to use to save this game.");
                Game.WriteLine("Type [cancel] to cancel the save operation.");

                GameState.PromptText = "Enter File Name: ";
                return;
            }

            if (!IsNewSaveFile && !sentOverwritePrompt)
            {
                sentOverwritePrompt = true;
                GameState.PromptText = $"Do you wish to overwrite your previous save of {GameState.SaveName}?";
                return;
            }
        }
    }
}