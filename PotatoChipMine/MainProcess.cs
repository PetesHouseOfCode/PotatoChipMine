using System;
using System.Collections.Generic;
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

        public MainProcess()
        {
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
                Mode = GameMode.Lobby,
                Running = true
            };
            _gameState.Store = new MinerStoreFactory(_gameUi, _gameState).BuildMineStore();
            _gameState.ControlRoom = new ControlRoomFactory(_gameUi, _gameState).BuildControlRoom();
            _gameState.Miner.Diggers = new List<ChipDigger>();
            _commandsGroup = new TopCommandGroupFactory(_gameUi).Build();
        }

        public void Run()
        {
            _gameUi.Intro();
            var intro = new[]
            {
                "Howdy Pilgrim.  Welcome to the 'tater chip mining game.",
                "You can buy needful things at the miner store.",
                "You can access the store by typing the command store."
            };

            _gameUi.ReportInfo(intro);
            while (_gameState.Running) AcceptCommand();
        }

        private void AcceptCommand()
        {
            var userCommand = _gameUi.AcceptUserCommand();
            _commandsGroup.ExecuteCommand(_gameUi, userCommand, _gameState);
        }
    }

    public class EmptyCommand : UserCommand
    {
    }

    public enum GameMode
    {
        Lobby = 1,
        Store = 2,
        Mining = 3,
        ControlRoom = 4
    }

    internal class ConsoleSpinner
    {
        private int _currentAnimationFrame;

        public ConsoleSpinner()
        {
            SpinnerAnimationFrames = new[]
            {
                "|",
                "/",
                "-",
                "\\"
            };
        }

        public string[] SpinnerAnimationFrames { get; set; }

        public void UpdateProgress()
        {
            // Store the current position of the cursor
            var originalX = Console.CursorLeft;
            var originalY = Console.CursorTop;

            // Write the next frame (character) in the spinner animation
            Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);

            // Keep looping around all the animation frames
            _currentAnimationFrame++;
            if (_currentAnimationFrame == SpinnerAnimationFrames.Length) _currentAnimationFrame = 0;

            // Restore cursor to original position
            Console.SetCursorPosition(originalX, originalY);
        }
    }
}