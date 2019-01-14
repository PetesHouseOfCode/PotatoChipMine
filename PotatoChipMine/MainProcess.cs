using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using PotatoChipMine.ControlRoom.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.Store;
using PotatoChipMine.Store.Services;

namespace PotatoChipMine
{
    public class MainProcess
    {
        private GameState _gameState;
        private readonly GameUI _gameUI;
        private readonly CommandsGroup _commandsGroup;

        public MainProcess()
        {
            _gameUI = new GameUI();
            _gameState = new GameState
            {
                Miner = new Miner
                {
                    Diggers = new List<ChipDigger>(),
                    TaterTokens = 100,
                    InventoryItems = new List<InventoryItem> { new InventoryItem { Name = "chips",Count=0,InventoryId = 0} }
                },
                Mode = GameMode.Lobby,
                Running = true
            };
            _gameState.Store = new MinerStoreFactory(_gameUI,_gameState).BuildMineStore();
            _gameState.ControlRoom = new ControlRoomFactory(_gameUI,_gameState).BuildControlRoom();
            _gameState.Miner.Diggers = new List<ChipDigger>();
            _commandsGroup = new TopCommandGroupFactory(_gameUI).Build();
        }

        public void Run()
        {
            var intro = new[]
            {
                "Howdy Pilgrim.  Welcome to the 'tater chip mining game.",
                "You can buy needful things at the miner store.",
                "You can access the store by typing the command store."
            };
            _gameUI.ReportInfo(intro);
            while (_gameState.Running)
            {
                AcceptCommand();
            }

        }

        private void AcceptCommand()
        {
         
            var userCommand = _gameUI.AcceptUserCommand();
            _commandsGroup.ExecuteCommand(_gameUI, userCommand,_gameState);
            //switch (userCommand.CommandText?.ToLower())
            //{
            //    case "help":
            //       _gameUI.ReportAvailableCommands(_commandsGroup.LocalCommands);
            //        break;
            //    case "dig":
            //        if (userCommand.Parameters.Count == 0)
            //        {
            //            RunDiggers(_miner.Diggers);
            //            break;
            //        }
            //        var turns = Convert.ToInt32(userCommand.Parameters[0]);
            //        RunDiggers(_miner.Diggers,turns);
            //        break;
            //    case "control-room":
            //        _controlRoom.EnterRoom();
            //        break;
            //    case "store":
            //        _minerStore.EnterRoom();
            //        break;
            //    case "inventory":
            //        _gameUI.ReportMinerInventory(_miner);
            //        break;
            //    case "vault":
            //        Console.WriteLine($"Chip Vault: {_miner.ChipVault}");
            //        break;
            //    case "miner":
            //        _gameUI.ReportMinerState(_miner);
            //        break;
            //    case "empty":
            //        EmptyHopper(userCommand.Parameters[0]);
            //        break;
            //    case "quit":
            //    case "end":
            //        _running = false;
            //        break;
            //    default:
            //        Console.WriteLine($"{userCommand?.CommandText} is not a valid command.");
            //        break;
            //}
        }
    }

    public class GameState
    {
        public Miner Miner { get; set; }
        public GameMode Mode { get; set; }
        public bool Running { get; internal set; }
        public MinerStore Store { get; internal set; }
        public ControlRoom.ControlRoom ControlRoom { get; set; }
    }

    public class CommandsDefinition
    {
        public string Command { get; set; }
        public string EntryDescription { get; set; }
        public string Description { get; set; }
        public Action<UserCommand,GameState> Execute { get; set; }
    }

    public class UserCommand
    {
        public string CommandText { get; set; }
        public List<string> Parameters { get; set; }
    }

    public class EmptyCommand : UserCommand
    {


    }
    public enum GameMode
    {
        Lobby=1,
        Store=2,
        Mining=3,
        ControlRoom=4
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
            if (_currentAnimationFrame == SpinnerAnimationFrames.Length)
            {
                _currentAnimationFrame = 0;
            }

            // Restore cursor to original position
            Console.SetCursorPosition(originalX, originalY);
        }
    }
}
