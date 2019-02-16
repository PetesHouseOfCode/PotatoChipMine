using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using PotatoChipMine.GameRooms;
using PotatoChipMine.GameRooms.ControlRoom.Services;
using PotatoChipMine.GameRooms.Store.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine
{
    public class MainProcess
    {
        private readonly CommandsGroup _commandsGroup;
        private readonly GameUI _gameUi;
        private readonly GameState _gameState;
        private readonly GamePersistenceService _gamePersistenceService = new GamePersistenceService();

        public MainProcess()
        {
            Console.CursorVisible = false;
            Console.Title = "Potato Chip Mine";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight - 30);
            
            _gameUi = new GameUI();
            
            _gameState = new GameState
            {
                Miner = new Miner
                {
                    Diggers = new List<ChipDigger>(),
                    TaterTokens = 100,
                    InventoryItems = new List<InventoryItem>
                        {new InventoryItem {Name = "chips", Count = 0, InventoryId = 0}}
                },
                Running = true
            };

            _gameState.Events.Add(new RestockingEvent(_gameState));
            _gameState.Events.Add(new LotteryEvent(_gameState));

            _commandsGroup = new TopCommandGroupFactory(_gameUi).Build();
            _gameState.Lobby = new LobbyRoom(_gameUi, _gameState, new[] { "Welcome to the Lobby" }, GameMode.Lobby, _commandsGroup);
            _gameState.Store = new MinerStoreFactory(_gameUi, _gameState, _commandsGroup).BuildMineStore();
            _gameState.ControlRoom = new ControlRoomFactory(_gameUi, _gameState, _commandsGroup).BuildControlRoom();
            _gameState.Miner.Diggers = new List<ChipDigger>();
            _gameState.SaveDirectory = @"c:\chipMiner\saves";
            Console.WindowWidth = 125;
        }

        public void GameLoop()
        {
            _gameUi.Intro();
            GameStartupRoutine();            
            _gameState.Lobby.EnterRoom();
            var frameTime = Stopwatch.StartNew();
            var gameTime = Stopwatch.StartNew();
            while (_gameState.Running)
            {
                var frame = Frame.NewFrame(gameTime.Elapsed, frameTime.Elapsed);
                Debug.WriteLine(frame.Elapsed);
                frameTime.Restart();
                var commands = GetInputs();
                ProcessCommands(commands);
                Update(frame);
                DoEvents();
            }
        }

        private IList<UserCommand> GetInputs()
        {
            return _gameUi.AcceptUserCommand(_gameState.CurrentRoom.Name);
        }

        private void ProcessCommands(IList<UserCommand> commands)
        {
            foreach (var command in commands)
            {
                _gameState.CurrentRoom.ExecuteCommand(command);
            }
        }

        public void Update(Frame frame)
        {
            foreach (var component in _gameState.Events)
                component.Update(frame);
        }

        private void DoEvents()
        {
            foreach (var newEvent in _gameState.NewEvents)
            {
                _gameUi.ReportEvent(newEvent.Message);
                _gameState.EventsHistory.Add(new EventLog
                {
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    Processed = DateTime.Now.ToString()
                });
            }

            _gameState.NewEvents = new List<GameEvent>();
        }

        private void GameStartupRoutine()
        {
            string newGame;
            do
            {
                _gameUi.FastWrite(new[] { "Do you want to start a new game?" });
                newGame = Console.ReadLine();
            } while (!newGame.Equals("yes", StringComparison.CurrentCultureIgnoreCase) &&
                     !newGame.Equals("no", StringComparison.CurrentCultureIgnoreCase));

            if (!newGame.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
            {
                if (Directory.Exists(_gameState.SaveDirectory))
                {
                    var name = _gameUi.CollectGameSaveToLoad(_gamePersistenceService.SaveFiles(_gameState));
                    _gamePersistenceService.LoadGame(_gameState, name);
                }
            }
            else
            {
                _gameUi.ReportInfo(new[]
                {
                    "Howdy pilgrim!  Welcome to glamorous world of 'tater chip mining!",
                    "I'm Earl, your mine bot. I'll be you're right hand man ... 'er bot, around this here mining operation.",
                    "Whats your name pilgrim?"
                });
                while (string.IsNullOrEmpty(_gameState.Miner.Name))
                {
                    _gameUi.WritePrompt("Enter Your Name");
                    var name = Console.ReadLine();
                    if (!string.IsNullOrEmpty(name))
                    {
                        _gameState.Miner.Name = name;
                    }
                }

                _gameUi.ReportInfo(new[]
                {
                    $"Very pleased to meet you {_gameState.Miner.Name}.",
                    "If you're new to 'tater mining you may want some instructions...",
                    "You look like maybe you know your way around a chip digger though.",
                    "Do you need instructions?"
                });
                var entry = "";
                while (entry != null &&
                       !entry.Equals("yes", StringComparison.CurrentCultureIgnoreCase) &&
                       !entry.Equals("no", StringComparison.CurrentCultureIgnoreCase))
                {
                    _gameUi.WritePrompt("Enter [yes] or [no]");
                    entry = Console.ReadLine();
                }

                if (entry.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    _gameUi.ReportInfo(new[]
                    {
                        "I'm sorry to report that the tutorial is under construction, but here's what we've got so far...",
                        Environment.NewLine,
                        ">]** You can type [help] at any time to see a list of available commands.",
                        Environment.NewLine,
                        "** You can buy and sell things in the store.",
                        "** Type [store] to enter the store.",
                        Environment.NewLine,
                        "** You can take actions related to your diggers in the control-room.",
                        "** Type [control-room] to enter the control-room",
                        Environment.NewLine
                    });
                    _gameUi.WritePrompt("<<Press Enter To Continue>>");
                    Console.ReadLine();
                }
            }
            _gameUi.ReportInfo(new[] { $"Well ok then.  Good luck to you {_gameState.Miner.Name}!" });
        }
    }
}