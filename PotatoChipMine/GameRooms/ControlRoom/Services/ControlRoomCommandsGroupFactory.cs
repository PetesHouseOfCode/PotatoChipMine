using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom.Services
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
                        Execute = DigHandler()
                    },
                    new CommandsDefinition()
                    {
                        Command = "equip",
                        Description = "Begins the process to equip a digger from your inventory to dig.",
                        Execute = EquipHandler()
                        
                    },
                    new CommandsDefinition()
                    {
                        Command = "empty",
                        EntryDescription = "empty [digger name]",
                        Description = "Empties the indicated diggers hopper into the chip vault.",
                        Execute = EmptyHandler()
                        
                    },
                    new CommandsDefinition()
                    {
                        Command = "scrap",
                        EntryDescription = "scrap [digger name]",
                        Description = "Destroys the digger indicated for bolts.",
                        Execute = ScrapHandler()
                    },
                    new CommandsDefinition()
                    {
                        Command = "repair",
                        EntryDescription = "repair [digger name]",
                        Description = "After confirmation repairs the digger to max durability for the quoted price.",
                        Execute = RepairHandler()
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

        private Action<UserCommand, GameState> RepairHandler()
        {
            return (userCommand, gameState) =>
            {
                var random = new Random();
                            
                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                if (digger == null)
                {
                    _gameUi.ReportException(new []{$"No digger named {userCommand.Parameters[0]} could be found."});
                }
                var responseList = new List<string>();
                var tokensCost = random.Next(1,10);
                var boltsCost =random.Next(1, 15);
                if (tokensCost > gameState.Miner.TaterTokens)
                {
                    responseList.Add("You don't have enough tokens.");
                }

                var bolts = gameState.Miner.Inventory("bolts");
                if (bolts==null || boltsCost > gameState.Miner.Inventory("bolts").Count)
                {
                    responseList.Add("You don't have enough bolts.");
                }

                if(responseList.Any())
                {
                    responseList.Insert(0,$"Repairs will cost {tokensCost} tater tokens and {boltsCost} bolts.");
                    _gameUi.ReportException(responseList.ToArray());
                }
                else
                {
                    if (!_gameUi.ConfirmDialog(new[]
                    {
                        $"Repairs will cost {tokensCost} tater tokens and {boltsCost} bolts.",
                        "Do you want to perform repairs"
                    })) return;
                    gameState.Miner.TaterTokens -= tokensCost;
                    gameState.Miner.Inventory("bolts").Count -= (int)boltsCost;
                    digger.Durability = digger.MaxDurability;
                }
            };
        }

        private Action<UserCommand, GameState> ScrapHandler()
        {
            return (userCommand, gameState) =>
            {
                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                if (digger == null)
                {
                    _gameUi.ReportException(new []{$"There are no diggers named {userCommand.Parameters[0]}."});
                    return;
                }
                gameState.Miner.Diggers.Remove(digger);
                var bolts = gameState.Miner.Inventory("bolts");
                if (bolts == null)
                {
                    bolts = new InventoryItem(){Name="bolts",Count = 0};
                    gameState.Miner.InventoryItems.Add(bolts);
                }

                var boltsReceived = new Random().Next(3,10);
                bolts.Count += boltsReceived;
                _gameUi.ReportDiggerScrapped(digger,boltsReceived);
            };
        }

        private Action<UserCommand, GameState> EmptyHandler()
        {
            return (userCommand, gameState) =>
            {
                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                var chips = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name == "chips");
                if (digger == null)
                {
                    Console.WriteLine($"Could not find digger named {userCommand.Parameters[0]}");
                    return;
                }

                var hopperCount = digger.Hopper.Count;
                if (chips != null) chips.Count += digger.Hopper.Count;
                digger.Hopper.Count = 0;
                _gameUi.ReportHopperEmptied(userCommand.Parameters[0],hopperCount,gameState.Miner.Inventory("chips").Count);
            };
        }

        private Action<UserCommand, GameState> EquipHandler()
        {
            return (userCommand, gameState) =>
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
            };
        }

        private Action<UserCommand, GameState> DigHandler()
        {
            return (userCommand, gameState) =>
            {
                if (userCommand.Parameters.Count == 0)
                {
                    DiggerRunnerService.RunDiggers(_gameUi, gameState);
                    return;
                }

                var turns = Convert.ToInt32(userCommand.Parameters[0]);
                DiggerRunnerService.RunDiggers(_gameUi,gameState, turns);
            };
        }
    }
}