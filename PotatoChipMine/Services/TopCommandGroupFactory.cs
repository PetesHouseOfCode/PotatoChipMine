using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PotatoChipMine.GameRooms.ControlRoom;
using PotatoChipMine.GameRooms.Store;
using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class TopCommandGroupFactory : ICommandGroupFactory
    {
        private readonly GameUI _gameUi;
        private readonly GamePersistenceService _gamePersistenceService;
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
                    Execute = (command, gameState) => { _gameUi.ReportAvailableCommands(commandsGroup, gameState); }
                },
                new CommandsDefinition
                {
                    Command = "store",
                    Description = "Opens the store mode and makes the store commands accessible.",
                    Execute = (command, gameState) =>
                    {
                        gameState.Mode = GameMode.Store;
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
                        gameState.Mode = GameMode.Lobby;
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
                string proceed;
                do
                {
                    _gameUi.FastWrite(new[]
                    {
                        "Loading a new game will end this game and overwrite it with the loaded game data.",
                        "Do you wish to proceed?"
                    });
                    proceed = Console.ReadLine() ?? string.Empty;
                    if (proceed.Equals("cancel", StringComparison.CurrentCultureIgnoreCase) ||
                        proceed.Equals("no", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _gameUi.ReportException(new[] {"Load cancelled."});
                        return;
                    }
                } while (!proceed.Equals("yes", StringComparison.CurrentCultureIgnoreCase));

                if (!userCommand.Parameters.Any())
                {
                    var name = _gameUi.CollectGameSaveToLoad(_gamePersistenceService.SaveFiles(gameState));
                    _gamePersistenceService.LoadGame(gameState, name);
                    _gameUi.FastWrite(new[] { $"{gameState.SaveName} loaded.  Good Luck {gameState.Miner.Name} !" });
                    return;
                }
                _gamePersistenceService.LoadGame(gameState, userCommand.Parameters[0]);
                _gameUi.FastWrite(new []{$"{gameState.SaveName} loaded.  Good Luck {gameState.Miner.Name} !"});
            };
        }

        private Action<UserCommand, GameState> SaveHandler()
        {
            return (userCommand, gameState) =>
            {
                bool isNew = false;
                while (true)
                {
                    if (gameState.SaveName == string.Empty)
                    {
                        isNew = true;
                        do
                        {
                            _gameUi.FastWrite(new[]
                            {
                                "You have not previously saved this game.",
                                "Enter the name you would like to use to save this game."
                            });
                            var saveName = Console.ReadLine()??string.Empty;
                            if (saveName.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _gameUi.ReportException(new[] { "Save cancelled." });
                                return;
                            }

                            gameState.SaveName = saveName;
                        } while (gameState.SaveName == string.Empty);
                    }
                    else
                    {
                        if (!isNew)
                        {
                            string proceed;
                            _gameUi.FastWrite(new[]
                                {$"Do you wish to overwrite your previous save of {gameState.SaveName}?"});
                            do
                            {
                                _gameUi.FastWrite(new[] {"Please enter [yes] or [no]."});
                                proceed = Console.ReadLine() ?? string.Empty;
                                if (proceed.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    _gameUi.ReportException(new[] {"Save cancelled."});
                                    return;
                                }
                            } while (!proceed.Equals("yes", StringComparison.CurrentCultureIgnoreCase) &&
                                     !proceed.Equals("no", StringComparison.CurrentCultureIgnoreCase));

                            if (proceed.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _gamePersistenceService.SaveGame(_gamePersistenceService.BuildFromGameState(gameState));
                                return;
                            }

                            _gameUi.FastWrite(new[] {"Enter the name you would like to use to save this game."});
                            var saveName = Console.ReadLine() ?? string.Empty;
                            if (saveName.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                            {
                                _gameUi.ReportException(new[] {"Save cancelled."});
                                return;
                            }
                            gameState.SaveName = saveName;
                        }
                        _gamePersistenceService.SaveGame(_gamePersistenceService.BuildFromGameState(gameState));
                        return;
                    }
                }
            };
        }

        private Action<UserCommand, GameState> TokensHandler()
        {
            return (userCommand, gameState) =>
            {
                _gameUi.ReportInfo(new[] {$"You have {gameState.Miner.TaterTokens} Tater Tokens"});
            };
        }
    }

    public class GamePersistenceService 
    {
        public GameSave BuildFromGameState(GameState gameState)
        {
            return new GameSave
            {
                Miner = gameState.Miner,
                Mode = gameState.Mode,
                SaveDirectory = gameState.SaveDirectory,
                SaveName = gameState.SaveName
            };
        }
        public void SaveGame(GameSave gameSave)
        {
            if (!Directory.Exists(gameSave.SaveDirectory)) Directory.CreateDirectory(gameSave.SaveDirectory);
            var stream = new StreamWriter(Path.Combine(gameSave.SaveDirectory, gameSave.SaveName + ".json"),
                false);
            stream.Write(JsonConvert.SerializeObject(gameSave));
            stream.Flush();
            stream.Close();
        }

        public void LoadGame(GameState gameState,string gameName)
        {
            if (Directory.Exists(gameState.SaveDirectory))
            {
                if (File.Exists(Path.Combine(gameState.SaveDirectory, $"{gameName}.json")))
                {
                    var str = File.ReadAllText(Path.Combine(gameState.SaveDirectory, $"{gameName}.json"));
                    var loadedGame = JsonConvert.DeserializeObject<GameSave>(str);
                    gameState.Miner = loadedGame.Miner;
                    gameState.Mode = loadedGame.Mode;
                    gameState.SaveDirectory = loadedGame.SaveDirectory;
                    gameState.SaveName = loadedGame.SaveName;
                }
            }
        }

        public FileInfo[] SaveFiles(GameState gameState)
        {
            var dirInfo = new DirectoryInfo(gameState.SaveDirectory);
            FileInfo[] files = dirInfo.GetFiles();
            return files;
        }
    }

    public class GameSave
    {
        public Miner Miner { get; set; }
        public GameMode Mode { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; }

    }
}