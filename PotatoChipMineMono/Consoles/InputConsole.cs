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
using SadConsole.Input;
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
    class InputConsole : ScrollingConsole
    {
        public string Prompt
        {
            get
            {
                return gameState.PromptText ?? $"{gameState.CurrentRoom.Name} Command >>" ?? string.Empty;
            }
        }

        private ClassicConsoleKeyboardHandler _keyboardHandlerObject;

        GameState gameState;
        readonly IPotatoChipGame game;

        public InputConsole(IPotatoChipGame game, GameState gameState)
            : base (125, 6)
        {
            this.game = game;
            this.gameState = gameState;

            this.gameState.PromptTextChanged += () =>
                {
                    //Prompt = gameState.PromptText ?? $"{gameState.CurrentRoom.Name} Command >>" ?? string.Empty;
                    //Prompt = Prompt + " ";
                    //Cursor.Print(Prompt);
                };

            _keyboardHandlerObject = new ClassicConsoleKeyboardHandler();

            // Assign our custom handler method from our handler object to this consoles keyboard handler.
            // We could have overridden the ProcessKeyboard method, but I wanted to demonstrate how you
            // can use your own handler on any console type.
            Components.Add(_keyboardHandlerObject);

            // Our custom handler has a call back for processing the commands the user types. We could handle
            // this in any method object anywhere, but we've implemented it on this console directly.
            _keyboardHandlerObject.EnterPressedAction = EnterPressedActionHandler;

            // Enable the keyboard and setup the prompt.
            UseKeyboard = true;
            Cursor.IsVisible = true;

            // Startup description
            ClearText();

            Cursor.Position = new Point(0, 0);
            _keyboardHandlerObject.CursorLastY = 0;
            TimesShiftedUp = 0;

            Cursor.DisableWordBreak = true;
            Cursor.Print(Prompt);
            Cursor.DisableWordBreak = false;
        }

        public void ClearText()
        {
            Clear();
            Cursor.Position = new Point(0, 0);
            _keyboardHandlerObject.CursorLastY = 0;
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

            foreach (var entity in game.CurrentScene.Entities)
            {
                entity.HandleInput(command);
            }
        }
    }
}
