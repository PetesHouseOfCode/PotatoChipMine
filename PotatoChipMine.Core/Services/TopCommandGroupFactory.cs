using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Services
{
    public class TopCommandGroupFactory : ICommandGroupFactory
    {
        private readonly GamePersistenceService _gamePersistenceService;

        public TopCommandGroupFactory()
        {
            _gamePersistenceService = new GamePersistenceService();
        }

        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup();
            commandsGroup.LocalCommands = new List<CommandsDefinition>
            {
                new CommandsDefinition
                {
                    CommandText = "help",
                    Description = "Display all of the commands available.",
                    Command = (userCommand, GameState) => new HelpCommand {GameState = GameState}
                },
                new CommandsDefinition
                {
                    CommandText = "store",
                    Description = "Opens the store mode and makes the store commands accessible.",
                    Execute = (command, gameState) => { gameState.Store.EnterRoom(); }
                },
                new CommandsDefinition
                {
                    CommandText = "miner",
                    Description = "Displays the miner's current chip vault, tater tokens, and diggers",
                    Execute = (userCommand, gameState) =>
                    {
                        var miner = gameState.Miner;
                        Game.WriteLine($"Name: {miner.Name}", PcmColor.Yellow);
                        Game.WriteLine($"Chip Vault:{miner.Inventory("chips").Count}", PcmColor.Yellow);
                        Game.WriteLine($"Tater Tokens:{miner.TaterTokens}", PcmColor.Yellow);
                        Game.WriteLine($"Diggers Count:{miner.Diggers.Count}", PcmColor.Yellow);
                        Game.WriteLine($"Lifetime Chips Dug:{miner.GetLifeTimeStat(Stats.LifetimeChips)}");
                    }
                },
                new CommandsDefinition
                {
                    CommandText = "vault",
                    Description = "Shows the number of chips currently in your vault.",
                    Execute = (userCommand, gameState) =>
                    {
                        Game.WriteLine($"Chip Vault: {gameState.Miner.Inventory("chips").Count}");
                    }
                },
                new CommandsDefinition
                {
                    CommandText = "tokens",
                    Description = "Shows then number of tokens you currently have.",
                    Execute = TokensHandler()
                },
                new CommandsDefinition
                {
                    CommandText = "exit",
                    Description = "Leaves the current room and returns you to the lobby.",
                    Execute = (userCommand, gameState) =>
                    {
                        if (gameState.Mode == GameMode.Lobby) return;
                        Game.WriteLine($"Leaving {gameState.Mode}...");
                        gameState.Lobby.EnterRoom();
                    }
                },
                new CommandsDefinition
                {
                    CommandText = "quit",
                    Description = "Ends the game.",
                    Execute = (userCommand, gameState) => { gameState.Running = false; }
                },
                new CommandsDefinition
                {
                    CommandText = "end",
                    Description = "Ends the game.",
                    Execute = (userCommand, gameState) => { gameState.Running = false; }
                },
                new CommandsDefinition
                {
                    CommandText = "inventory",
                    Description = "Shows the miners items inventory",
                    Execute = (userCommand, gameState) =>
                    {
                        var table = new TableOutput(80);
                        table.AddHeaders("Name", "Quantity");
                        foreach (var minerInventoryItem in gameState.Miner.InventoryItems)
                        {
                            table.AddRow(minerInventoryItem.Name, minerInventoryItem.Count.ToString());
                        }

                        Game.Write(table);
                    }
                },
                new CommandsDefinition
                {
                    CommandText = "control-room",
                    Description = "Enter the control room. Control room commands become available",
                    Execute = (userCommand, gameState) => { gameState.ControlRoom.EnterRoom(); }
                },
                new CommandsDefinition
                {
                    CommandText = "diggers",
                    Description = "Displays a list of all of the miner's equipped diggers.",
                    Execute = (userCommand, gameState) =>
                    {
                        var table = new TableOutput(80, PcmColor.Yellow);
                        table.AddHeaders("Name", "Durability","Density", "Hardness", "Hopper Space");
                        foreach (var digger in gameState.Miner.Diggers)
                        {
                            table.AddRow(digger.Name,
                                digger.DiggerBit.Durability.ToString(),
                                digger.MineSite.ChipDensity.ToString(),
                                digger.MineSite.Hardness.ToString(),
                                $"{digger.Hopper.Max - digger.Hopper.Count}/{digger.Hopper.Max}");
                        }

                        Game.Write(table);
                    }
                },
                new CommandsDefinition
                {
                    CommandText = "save",
                    Description = "Saves the current game.",
                    Execute = SaveHandler()
                },
                new CommandsDefinition
                {
                    CommandText = "load",
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
                Game.PushScene(Scene.Create(new LoadGameEntity(gameState, _gamePersistenceService)));
            };
        }

        private Action<UserCommand, GameState> SaveHandler()
        {
            return (userCommand, gameState) =>
            {
                Game.PushScene(Scene.Create(new SaveGameEntity(gameState, _gamePersistenceService)));
            };
        }

        private Action<UserCommand, GameState> TokensHandler()
        {
            return (userCommand, gameState) =>
            {
                Game.WriteLine($"You have {gameState.Miner.TaterTokens} Tater Tokens", PcmColor.Green);
            };
        }
    }
}