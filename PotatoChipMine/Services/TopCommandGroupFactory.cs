using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class TopCommandGroupFactory : ICommandGroupFactory
    {
        private readonly GamePersistenceService _gamePersistenceService;
        private readonly GameUI _gameUi;

        public TopCommandGroupFactory(GameUI gameUi)
        {
            _gameUi = gameUi;
            _gamePersistenceService = new GamePersistenceService();
        }

        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup();
            commandsGroup.LocalCommands = new List<CommandsDefinition>
            {
                new CommandsDefinition
                {
                    Command = "help",
                    Description = "Display all of the commands available.",
                    Execute = (command, gameState) => { _gameUi.ReportAvailableCommands(gameState); }
                },
                new CommandsDefinition
                {
                    Command = "store",
                    Description = "Opens the store mode and makes the store commands accessible.",
                    Execute = (command, gameState) =>
                    {
                        gameState.Store.EnterRoom();
                    }
                },
                new CommandsDefinition
                {
                    Command = "miner",
                    Description = "Displays the miner's current chip vault, tater tokens, and diggers",
                    Execute = (userCommand, gameState) => { _gameUi.ReportMinerState(gameState.Miner); }
                },
                new CommandsDefinition
                {
                    Command = "vault",
                    Description = "Shows the number of chips currently in your vault.",
                    Execute = (userCommand, gameState) => { _gameUi.ReportVault(gameState); }
                },
                new CommandsDefinition
                {
                    Command = "tokens",
                    Description = "Shows then number of tokens you currently have.",
                    Execute = TokensHandler()
                },
                new CommandsDefinition
                {
                    Command = "exit",
                    Description = "Leaves the current room and returns you to the lobby.",
                    Execute = (userCommand, gameState) =>
                    {
                        if (gameState.Mode == GameMode.Lobby) return;
                        _gameUi.FastWrite(new[] {$"Leaving {gameState.Mode}..."});
                        gameState.Lobby.EnterRoom();
                    }
                },
                new CommandsDefinition
                {
                    Command = "quit",
                    Description = "Ends the game.",
                    Execute = (userCommand, gameState) => { gameState.Running = false; }
                },
                new CommandsDefinition
                {
                    Command = "end",
                    Description = "Ends the game.",
                    Execute = (userCommand, gameState) => { gameState.Running = false; }
                },
                new CommandsDefinition
                {
                    Command = "inventory",
                    Description = "Shows the miners items inventory",
                    Execute = (userCommand, gameState) => { _gameUi.ReportMinerInventory(gameState.Miner); }
                },
                new CommandsDefinition
                {
                    Command = "control-room",
                    Description = "Enter the control room. Control room commands become available",
                    Execute = (userCommand, gameState) => { gameState.ControlRoom.EnterRoom(); }
                },
                new CommandsDefinition
                {
                    Command = "diggers",
                    Description = "Displays a list of all of the miner's equipped diggers.",
                    Execute = (userCommand, gameState) => { _gameUi.ReportDiggers(gameState.Miner.Diggers); }
                },
                new CommandsDefinition
                {
                    Command = "save",
                    Description = "Saves the current game.",
                    Execute = SaveHandler()
                },
                new CommandsDefinition
                {
                    Command = "load",
                    EntryDescription = "load || load [save name]",
                    Description = "Loads shows games available to load, or loads the indicated saved game.",
                    Execute = LoadHandler()
                }
            };
            return commandsGroup;
        }

        private Action<UserCommand, GameState> LoadHandler()
        {
            return (userCommand, gameState) =>
            {
                if (!_gameUi.ConfirmDialog(new[]
                {
                    "Loading a new game will end this game and overwrite it with the loaded game data.",
                    "Do you wish to proceed?"
                }))
                    return;
                if (!userCommand.Parameters.Any())
                {
                    var name = _gameUi.CollectGameSaveToLoad(_gamePersistenceService.SaveFiles(gameState));
                    _gamePersistenceService.LoadGame(gameState, name);
                    _gameUi.FastWrite(new[] {$"{gameState.SaveName} loaded.  Good Luck {gameState.Miner.Name} !"});
                    return;
                }

                _gamePersistenceService.LoadGame(gameState, userCommand.Parameters[0]);
                _gameUi.FastWrite(new[] {$"{gameState.SaveName} loaded.  Good Luck {gameState.Miner.Name} !"});
            };
        }

        private Action<UserCommand, GameState> SaveHandler()
        {
            return (userCommand, gameState) =>
            {
                
                SetSaveName(gameState);
                _gamePersistenceService.SaveGame(_gamePersistenceService.BuildFromGameState(gameState));
            };
        }

        private Action<UserCommand, GameState> TokensHandler()
        {
            return (userCommand, gameState) =>
            {
                _gameUi.ReportInfo(new[] {$"You have {gameState.Miner.TaterTokens} Tater Tokens"});
            };
        }

        private void SetSaveName(GameState gameState)
        {
            var isNew = false;
            while (true)
                if (gameState.SaveName == string.Empty)
                {
                    isNew = true;
                    var saveName = _gameUi.SavePrompt(true);
                    if (saveName.Equals("cancel", StringComparison.CurrentCultureIgnoreCase)) return;
                    gameState.SaveName = saveName;
                }
                else
                {
                    if (isNew)
                    {

                        return;
                    }

                    if (_gameUi.ConfirmDialog(new[]
                        {$"Do you wish to overwrite your previous save of {gameState.SaveName}?"}))
                    {
                        return;
                    }

                    var saveName = _gameUi.SavePrompt(false);
                    if (saveName == string.Empty) return;
                    gameState.SaveName = saveName;
                }
        }

    }
}