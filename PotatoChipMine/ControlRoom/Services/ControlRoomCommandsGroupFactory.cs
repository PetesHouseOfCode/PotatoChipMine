using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.ControlRoom.Services
{
    public class ControlRoomCommandsGroupFactory
    {
        private readonly GameUI _gameUi;

        public ControlRoomCommandsGroupFactory(GameUI gameUi)
        {
            _gameUi = gameUi;
        }

        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup()
            {
                ParentGroup = new TopCommandGroupFactory(_gameUi).Build(),
                LocalCommands = new List<CommandsDefinition>()
                {
                    new CommandsDefinition()
                    {
                        Command = "dig",
                        EntryDescription = "dig || dig [number of digs]",
                        Description = "Runs all equipment for 5 cycles or the number of digs indicated.",
                        Execute = (userCommand, gameState) =>
                        {
                            if (userCommand.Parameters.Count == 0)
                            {
                                DiggerRunnerService.RunDiggers(_gameUi, gameState);
                                return;
                            }

                            var turns = Convert.ToInt32(userCommand.Parameters[0]);
                            DiggerRunnerService.RunDiggers(_gameUi,gameState, turns);
                        }
                    },
                    new CommandsDefinition()
                    {
                        Command = "equip",
                        Description = "Begins the process to equip a digger from your inventory to dig.",
                        Execute = (userCommand, gameState) =>
                        {
                            var digger = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name.ToLower() == "digger");
                            if (digger != null && digger.Count > 0)
                            {
                                var factory = new MineSiteFactory();
                                var newDigger = new ChipDigger(factory.BuildSite()) { Durability = 20 };
                                _gameUi.WritePrompt("enter digger name");
                                newDigger.Name = Console.ReadLine()?.Trim().Replace(" ", "-");
                                digger.Count--;
                                gameState.Miner.Diggers.Add(newDigger);
                                _gameUi.ReportDiggerEquipped(newDigger.Name);
                                return;
                            }

                            _gameUi.ReportException(new[] {"You don't have any diggers in your inventory!"});
                        }
                        
                    },
                    new CommandsDefinition()
                    {
                        Command = "empty",
                        EntryDescription = "empty [digger name]",
                        Description = "Empties the indicated diggers hopper into the chip vault.",
                        Execute = (userCommand, gameState) =>
                        {
                            var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                                x.Name.ToUpper() == userCommand.Parameters[0].ToUpper());
                            var chips = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name == "chips");
                            if (digger == null)
                            {
                                Console.WriteLine($"Could not find digger named {userCommand.Parameters[0]}");
                                return;
                            }

                            var hopperCount = digger.Hopper.Count;
                            chips.Count += digger.Hopper.Count;
                            digger.Hopper.Count = 0;
                            _gameUi.ReportHopperEmptied(userCommand.Parameters[0],hopperCount,gameState.Miner.Inventory("chips").Count);
                        }
                        
                    }
                }

            };
            commandsGroup.LocalCommands.Add(new CommandsDefinition()
            {
                Command = "help",
                Description = "Shows a description of all the currently available commands.",
                Execute = (userCommand, gameState) => {
                    _gameUi.ReportAvailableCommands(commandsGroup, gameState);
                }
            });
            return commandsGroup;
        }
    }
}