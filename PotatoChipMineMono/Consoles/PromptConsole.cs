using Microsoft.Xna.Framework;
using PotatoChipMine.Core;
using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms;
using PotatoChipMine.Core.GameRooms.ControlRoom.Services;
using PotatoChipMine.Core.GameRooms.Store.Services;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using PotatoChipMineMono.Components;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventLog = PotatoChipMine.Core.EventLog;
using MineGame = PotatoChipMine.Core.GameEngine.Game;

namespace PotatoChipMineMono.Consoles
{
    class PromptConsole : ScrollingConsole, IPotatoChipGame
    {
        public string Prompt { get; set; }

        private ClassicConsoleKeyboardHandler _keyboardHandlerObject;

        bool isShowingPrompt;
        private readonly CommandsGroup _commandsGroup;
        private readonly GameState _gameState;
        private readonly GamePersistenceService _gamePersistenceService = new GamePersistenceService();
        Stopwatch gameTime = Stopwatch.StartNew();
        public Stack<Scene> SceneStack { get; } = new Stack<Scene>();
        public Scene CurrentScene { get; set; }
        public ConsoleBuffer Output { get; set; } = new ConsoleBuffer();


        public PromptConsole()
            : base(125, 40)
        {
            _gameState = new GameState
            {
                Running = true
            };
            _gameState.PromptTextChanged += () => { Prompt = _gameState.PromptText ?? $"{_gameState.CurrentRoom.Name} Command >>" ?? string.Empty; };


            _commandsGroup = new TopCommandGroupFactory().Build();
            _gameState.Lobby = new LobbyRoom(_gameState, new[] { "Welcome to the Lobby" }, GameMode.Lobby, _commandsGroup);
            _gameState.Store = new MinerStoreFactory(_gameState, _commandsGroup).BuildMineStore();
            _gameState.ControlRoom = new ControlRoomFactory(_gameState, _commandsGroup).BuildControlRoom();
            _gameState.SaveDirectory = @"c:\chipMiner\saves";

            IsVisible = false;
            IsFocused = false;

            _keyboardHandlerObject = new ClassicConsoleKeyboardHandler();

            // Assign our custom handler method from our handler object to this consoles keyboard handler.
            // We could have overridden the ProcessKeyboard method, but I wanted to demonstrate how you
            // can use your own handler on any console type.
            Components.Add(_keyboardHandlerObject);

            // Our custom handler has a call back for processing the commands the user types. We could handle
            // this in any method object anywhere, but we've implemented it on this console directly.
            _keyboardHandlerObject.EnterPressedAction = EnterPressedActionHandler;

            StartGame();
            // Enable the keyboard and setup the prompt.
            UseKeyboard = true;
            Cursor.IsVisible = false;
            Prompt = _gameState.PromptText ?? "";


            // Startup description
            ClearText();
            Cursor.Position = new Point(0, 0);
            _keyboardHandlerObject.CursorLastY = 0;
            TimesShiftedUp = 0;
        }

        public void StartGame()
        {
            MineGame.SetMainProcess(this);

            _gameState.Lobby.EnterRoom();
            MineGame.SwitchScene(Scene.Create(new List<IGameEntity>{
                new GameLoaderEntity(_gameState)
            }));
        }

        public void ClearText()
        {
            Clear();
            Cursor.Position = new Point(0, 0);
            _keyboardHandlerObject.CursorLastY = 0;
        }

        public override void Update(TimeSpan timeElapsed)
        {
            var frame = Frame.NewFrame(gameTime.Elapsed, timeElapsed);
            foreach (var entity in CurrentScene.Entities)
                entity.Update(frame);

            DoEvents();
            base.Update(timeElapsed);
        }

        private void DoEvents()
        {
            foreach (var newEvent in _gameState.NewEvents)
            {
                MineGame.WriteLine(newEvent.Message + Environment.NewLine, PcmColor.Green);
                _gameState.EventsHistory.Add(new EventLog
                {
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    Processed = DateTime.Now.ToString()
                });
            }

            _gameState.NewEvents.Clear();
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var characters = Output.Read(3).ToList();
            if (characters.Any())
            {
                HideCommandPrompt();
                foreach (var character in characters)
                    Cursor.Print(new ColoredString(character.Char.ToString(), character.ForegroundColor.ToColor(), character.BackgroundColor.ToColor()));
            }
            else
            {
                ShowCommandPrompt();
            }

            base.Draw(timeElapsed);
        }

        void HideCommandPrompt()
        {
            if (!isShowingPrompt)
                return;

            isShowingPrompt = false;
            Cursor.DisableWordBreak = true;
            Cursor.Print("\r                                                                    \r");
            Cursor.DisableWordBreak = false;
        }

        void ShowCommandPrompt()
        {
            if (isShowingPrompt)
                return;

            isShowingPrompt = true;
            Cursor.DisableWordBreak = true;
            Cursor.Print(Prompt);
            Cursor.DisableWordBreak = false;
        }

        private void EnterPressedActionHandler(string value)
        {
            var commandEntry = value.Trim().Split(' ');
            if (commandEntry == null)
            {
                return;
            }

            if (commandEntry.Length == 0)
            {
                return;
            }

            var command = new UserCommand { CommandText = commandEntry?[0], Parameters = commandEntry.Skip(1).ToList() };

            foreach (var entity in CurrentScene.Entities)
            {
                entity.HandleInput(command);
            }

            //            if (value.ToLower() == "help")
            //            {
            //                Cursor.NewLine().
            //                              Print("  Advanced Example: Command Prompt - HELP").NewLine().
            //                              Print("  =======================================").NewLine().NewLine().
            //                              Print("  help      - Display this help info").NewLine().
            //                              Print("  ver       - Display version info").NewLine().
            //                              Print("  cls       - Clear the screen").NewLine().
            //                              Print("  look      - Example adventure game cmd").NewLine().
            //                              Print("  exit,quit - Quit the program").NewLine().
            //                              Print("  ").NewLine();
            //            }
            //            else if (value.ToLower() == "ver")
            //                Cursor.Print("  SadConsole for MonoGame").NewLine();

            //            else if (value.ToLower() == "cls")
            //                ClearText();

            //            else if (value.ToLower() == "look")
            //                Cursor.Print("  Looking around you discover that you are in a dark and empty room. To your left there is a computer monitor in front of you and Visual Studio is opened, waiting for your next command.").NewLine();

            //            else if (value.ToLower() == "exit" || value.ToLower() == "quit")
            //            {
            //#if WINDOWS_UAP
            //                //Windows.UI.Xaml.Application.Current.Exit();       Not working?
            //#else
            //                Environment.Exit(0);
            //#endif
            //            }

            //            else
            //                Cursor.Print("  Unknown command").NewLine();
        }
    }

    public static class PcmColorExtensions
    {
        public static Color ToColor(this PcmColor pcmColor)
        {
            return new Color(pcmColor.R, pcmColor.G, pcmColor.B, pcmColor.A);
        }
    }
}
