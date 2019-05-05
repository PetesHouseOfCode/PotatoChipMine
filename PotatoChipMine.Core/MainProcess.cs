using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using PotatoChipMine.Core.GameRooms;
using PotatoChipMine.Core.GameRooms.ControlRoom.Services;
using PotatoChipMine.Core.GameRooms.Store.Services;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Entities;

namespace PotatoChipMine.Core
{
    public interface IPotatoChipGame
    {
        void StartGame();

        Scene CurrentScene { get; set; }
        ConsoleBuffer Output { get; set; }
        Stack<Scene> SceneStack { get; }
    }

    public class MainProcess : IPotatoChipGame
    {
        private readonly CommandsGroup _commandsGroup;
        private readonly GameUI _gameUi;
        private readonly GameState _gameState;
        private readonly GamePersistenceService _gamePersistenceService = new GamePersistenceService();
        public Stack<Scene> SceneStack { get; } = new Stack<Scene>();
        public Scene CurrentScene { get; set; }

        public ConsoleBuffer Output { get; set; } = new ConsoleBuffer();

        public MainProcess()
        {
            Console.CursorVisible = false;
            Console.Title = "Potato Chip Mine";
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight - 30);

            _gameState = new GameState
            {
                Running = true
            };
            _gameUi = new GameUI(_gameState);


            _commandsGroup = new TopCommandGroupFactory(_gameUi).Build();
            _gameState.Lobby = new LobbyRoom(_gameState, new[] { "Welcome to the Lobby" }, GameMode.Lobby, _commandsGroup);
            _gameState.Store = new MinerStoreFactory(_gameUi, _gameState, _commandsGroup).BuildMineStore();
            _gameState.ControlRoom = new ControlRoomFactory(_gameUi, _gameState, _commandsGroup).BuildControlRoom();
            _gameState.SaveDirectory = @"c:\chipMiner\saves";

            Console.WindowWidth = 125;
        }

        public void StartGame()
        {
            Game.SetMainProcess(this);
            _gameUi.Intro();

            _gameState.Lobby.EnterRoom();
            Game.SwitchScene(Scene.Create(new List<IGameEntity>{
                new GameLoaderEntity(_gameState)
            }));
            GameLoop();
        }

        private void GameLoop()
        {
            var frameTime = Stopwatch.StartNew();
            var gameTime = Stopwatch.StartNew();
            while (_gameState.Running)
            {
                var frame = Frame.NewFrame(gameTime.Elapsed, frameTime.Elapsed);
                frameTime.Restart();
                var commands = GetInputs();
                ProcessCommands(commands);
                Update(frame);

                // TODO: Move to Entity
                CalculateFrameRate(frame);
                DoEvents();
                Draw(frame);
            }
        }

        private int fpsFrames = 0;
        private TimeSpan fpsElapsed = TimeSpan.Zero;
        private double fpsAvg = 0;

        private void CalculateFrameRate(Frame frame)
        {
            if (fpsFrames == 0)
            {
                fpsElapsed = frame.TimeSinceStart;
            }

            fpsFrames++;

            if (!(frame.TimeSinceStart.Subtract(fpsElapsed).TotalSeconds >= 1))
                return;

            var fps = fpsFrames / (frame.TimeSinceStart.Subtract(fpsElapsed).TotalSeconds);
            fpsAvg = ApproxRollingAverage(fpsAvg, fps);
            Console.Title = $"Miner - fps: {fpsAvg:0.##}";
            fpsFrames = 0;
        }

        private double ApproxRollingAverage(double avg, double newSample)
        {
            avg -= avg / 10;
            avg += newSample / 10;
            return avg;
        }

        private IList<UserCommand> GetInputs()
        {
            return _gameUi.AcceptUserCommand();
        }

        private void ProcessCommands(IList<UserCommand> commands)
        {
            foreach (var command in commands)
            {
                foreach (var entity in CurrentScene.Entities)
                {
                    entity.HandleInput(command);
                }
            }
        }

        private void Update(Frame frame)
        {
            foreach (var entity in CurrentScene.Entities)
                entity.Update(frame);
        }

        private void DoEvents()
        {
            foreach (var newEvent in _gameState.NewEvents)
            {
                Game.WriteLine(newEvent.Message + Environment.NewLine, ConsoleColor.Green);
                _gameState.EventsHistory.Add(new EventLog
                {
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    Processed = DateTime.Now.ToString()
                });
            }

            _gameState.NewEvents.Clear();
        }

        private TimeSpan lastCharOut = TimeSpan.Zero;

        private void Draw(Frame frame)
        {
            if (lastCharOut != TimeSpan.Zero && frame.TimeSinceStart.Subtract(lastCharOut).Milliseconds < 3)
            {
                return;
            }

            var character = Output.Read();
            if (character != null)
            {
                lastCharOut = frame.TimeSinceStart;
                _gameUi.HideCommandPrompt();
                Console.ForegroundColor = character.ForegroundColor;
                Console.BackgroundColor = character.BackgroundColor;
                Console.Write(character.Char);
            }
            else
            {
                lastCharOut = TimeSpan.Zero;
                _gameUi.ShowCommandPrompt();
            }
        }
    }
}