using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class TopCommandGroupFactory:ICommandGroupFactory
    {
        private readonly GameUI _gameUI;

        public TopCommandGroupFactory(GameUI gameUi)
        {
            _gameUI = gameUi;
        }
        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup();
                commandsGroup.LocalCommands = new List<CommandsDefinition>(){
                new CommandsDefinition
                {
                    Command = "help",
                    Description = "Display all of the commands available.",
                    Execute = (command, gameState)=>
                    {
                        _gameUI.ReportAvailableCommands(commandsGroup,gameState);
                    }
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
                    Execute = (userCommand,gameState) =>
                    {
                        _gameUI.ReportMinerState(gameState.Miner);
                    }
                },
                new CommandsDefinition
                {
                    Command = "vault",
                    Description = "Shows the number of chips currently in your vault.",
                    Execute = (userCommand,gameState) =>
                    {
                        _gameUI.ReportVault(gameState);
                    }
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
                        _gameUI.FastWrite(new []{$"Leaving {gameState.Mode}..."});
                        gameState.Mode = GameMode.Lobby;
                    }
                },
                new CommandsDefinition
                {
                    Command = "quit",
                    Description = "Ends the game.",
                    Execute = (userCommand,gameState) => {gameState.Running  = false; }
                },
                new CommandsDefinition
                {
                    Command = "end",
                    Description = "Ends the game.",
                    Execute = (userCommand,gameState) => {gameState.Running  = false; }
                },
                new CommandsDefinition
                {
                    Command = "inventory",
                    Description = "Shows the miners items inventory",
                    Execute = (userCommand, gameState) =>
                    {
                        _gameUI.ReportMinerInventory(gameState.Miner);
                    }
                },
                new CommandsDefinition
                {
                    Command = "control-room",
                    Description = "Enter the control room. Control room commands become available",
                    Execute = (userCommand, gameState) => { gameState.ControlRoom.EnterRoom(); }
                }
            };
            return commandsGroup;
        }

        private Action<UserCommand, GameState> TokensHandler()
        {
            return (userCommand, gameState) =>
                {
                    _gameUI.ReportInfo(new[] {$"You have {gameState.Miner.TaterTokens} Tater Tokens"});
                };
        }
    }
}