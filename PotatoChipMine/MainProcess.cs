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
            GameStartupRoutine();
            while (_gameState.Running) AcceptCommand();
        }

        private void AcceptCommand()
        {
            var userCommand = _gameUi.AcceptUserCommand();
            _commandsGroup.ExecuteCommand(_gameUi, userCommand, _gameState);
        }

        private void GameStartupRoutine()
        {
            _gameUi.ReportInfo(new []
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
            _gameUi.ReportInfo(new []
            {
                $"Very pleased to meet you {_gameState.Miner.Name}.",
                "If you're new to 'tater mining you may want some instructions...",
                "You look like maybe you know your way around a chip digger though.",
                "Do you need instructions?"
            });
            var entry = "";
            while (entry != null &&
                   !entry.Equals("yes",StringComparison.CurrentCultureIgnoreCase) &&  
                   !entry.Equals("no",StringComparison.CurrentCultureIgnoreCase))
            {
                _gameUi.WritePrompt("Enter [yes] or [no]");
                entry = Console.ReadLine();
            }

            if (entry.Equals("yes",StringComparison.CurrentCultureIgnoreCase))
            {
                _gameUi.ReportInfo(new []{
                    "I'm sorry to report that the tutorial is under construction, but here's what we've got so far...",
                    Environment.NewLine,
                    ">]** You can type [help] at any time to see a list of available commands.",
                    Environment.NewLine,
                    "** You can buy and sell things in the store.",
                    "** Type [store] to enter the store.",
                    Environment.NewLine,
                    "** You can take actions related to your diggers in the control-room.",
                    "** Type [control-room] to enter the control-room",
                    Environment.NewLine});
                _gameUi.WritePrompt("<<Press Enter To Continue>>");
                Console.ReadLine();
            }
            else
            {
                _gameUi.ReportInfo(new []{$"Well ok then.  Good luck to you {_gameState.Miner.Name}!"});
            }
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