using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.ControlRoom.Services
{
    public class ControlRoomCommandsGroupFactory
    {
        public CommandsGroup Build()
        {
            var commandsGroup = new CommandsGroup()
            {
                LocalCommands = new List<CommandsDefinition>()
                {
                    //new CommandsDefinition()
                    //{
                    //    Command = "dig",
                    //    EntryDescription = "dig || dig [number of digs]",
                    //    Description = "Runs all equipment for 5 cycles or the number of digs indicated.",
                    //    Execute = DigHandler()
                    //},
                    new CommandsDefinition()
                    {
                        CommandText = "equip",
                        Description = "Begins the process to equip a digger from your inventory to dig.",
                        Execute = EquipHandler()
                        
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "empty",
                        EntryDescription = "empty [digger name]",
                        Description = "Empties the indicated diggers hopper into the chip vault.",
                        Execute = EmptyHandler()
                        
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "scrap",
                        EntryDescription = "scrap [digger name]",
                        Description = "Destroys the digger indicated for bolts.",
                        Execute = ScrapHandler()
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "repair",
                        EntryDescription = "repair [digger name]",
                        Description = "After confirmation repairs the digger to max durability for the quoted price.",
                        Execute = RepairHandler()
                    }
                }

            };
            //commandsGroup.LocalCommands.Add(new CommandsDefinition()
            //{
            //    Command = "help",
            //    Description = "Shows a description of all the currently available commands.",
            //    Execute = (userCommand, gameState) => {
            //        _gameUi.ReportAvailableCommands(gameState);
            //    }
            //});
            return commandsGroup;
        }

        private Action<UserCommand, GameState> RepairHandler()
        {
            return (userCommand, gameState) =>
            {
                if (!userCommand.Parameters.Any())
                {
                    Game.WriteLine("You will need to provide a digger name!", PcmColor.Red);
                    return;
                }

                var random = new Random();

                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                if (digger == null)
                {
                    Game.WriteLine($"No digger named {userCommand.Parameters[0]} could be found.", PcmColor.Red);
                    return;
                }

                var responseList = new List<string>();
                var tokensCost = random.Next(1, 10);
                var boltsCost = random.Next(1, 15);
                if (tokensCost > gameState.Miner.TaterTokens)
                {
                    responseList.Add("You don't have enough tokens.");
                }

                var bolts = gameState.Miner.Inventory("bolts");
                if (bolts == null || boltsCost > gameState.Miner.Inventory("bolts").Count)
                {
                    responseList.Add("You don't have enough bolts.");
                }

                if (responseList.Any())
                {
                    responseList.Insert(0, $"Repairs will cost {tokensCost} tater tokens and {boltsCost} bolts.");
                    Game.WriteLine(string.Join(Environment.NewLine, responseList.ToArray()), PcmColor.Red);
                }
                else
                {
                    var scene = Scene.Create(new List<IGameEntity>{
                        new RepairHandlerEntity(gameState)
                        {
                            Digger = digger,
                            TokenCost = tokensCost,
                            BoltsCost = boltsCost
                        }
                    });

                    gameState.PromptText = "Enter Digger Name: ";
                    Game.PushScene(scene);
                }
            };
        }

        private Action<UserCommand, GameState> ScrapHandler()
        {
            return (userCommand, gameState) =>
            {
                if (!userCommand.Parameters.Any())
                {
                    Game.WriteLine("You will need to provide a digger name!", PcmColor.Red);
                    return;
                }
                
                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                if (digger == null)
                {
                    Game.WriteLine($"There are no diggers named {userCommand.Parameters[0]}.", PcmColor.Red);
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
                Game.WriteLine($"{digger.Name} was scrapped for {boltsReceived} bolts.", PcmColor.Yellow);
            };
        }

        private Action<UserCommand, GameState> EmptyHandler()
        {
            return (userCommand, gameState) =>
            {
                if (!userCommand.Parameters.Any())
                {
                    Game.WriteLine("You will need to provide a digger name!", PcmColor.Red);
                    return;
                }
                
                var diggerName = userCommand.Parameters[0];
                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                var chips = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name == "chips");
                if (digger == null)
                {
                    Game.WriteLine($"Could not find digger named {userCommand.Parameters[0]}", PcmColor.Red);
                    return;
                }

                var hopperCount = digger.Hopper.Count;
                if (chips != null)
                {
                    chips.Count += digger.Hopper.Count;
                    gameState.Miner.LifetimeChips += digger.Hopper.Count;
                }

                digger.Hopper.Empty();

                Game.WriteLine($"{hopperCount} was removed from {diggerName}'s hopper and moved into the chip vault.",
                    PcmColor.Yellow);
                Game.WriteLine($"Vault Chips:{gameState.Miner.Inventory("chips").Count}", PcmColor.Yellow);
            };
        }

        private Action<UserCommand, GameState> EquipHandler()
        {
            return (userCommand, gameState) =>
            {
                var scene = Scene.Create(new List<IGameEntity>{
                    new EquipHandlerEntity(gameState)
                });

                var digger = gameState.Miner.InventoryItems.FirstOrDefault(x => x.Name.ToLower() == "digger");
                if (digger != null && digger.Count > 0)
                {
                    gameState.PromptText = "Enter Digger Name: ";
                    Game.PushScene(scene);
                    return;
                }

                Game.WriteLine("You don't have any diggers in your inventory!", PcmColor.Red);
            };
        }

    //    private Action<UserCommand, GameState> DigHandler()
    //    {
    //        return (userCommand, gameState) =>
    //        {
    //            if (userCommand.Parameters.Count == 0)
    //            {
    //                DiggerRunnerService.RunDiggers(_gameUi, gameState);
    //                return;
    //            }
                
    //            var turns = Convert.ToInt32(userCommand.Parameters[0]);
    //            DiggerRunnerService.RunDiggers(_gameUi,gameState, turns);
    //        };
    //    }
    }
}