using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

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
                    new CommandsDefinition()
                    {
                        CommandText = "equip",
                        Description = "Begins the process to equip a digger from your inventory to dig.",
                        Command =  (userCommand, gameState) => new EquipCommand{GameState = gameState}

                    },
                    new CommandsDefinition()
                    {
                        CommandText = "empty",
                        EntryDescription = "empty [digger name]",
                        Description = "Empties the indicated diggers hopper into the chip vault.",
                        Command = (userCommand, gameState) =>
                        {
                            if (!userCommand.Parameters.Any())
                            {
                                return new FailedMessageCommand("You will need to provide a digger name!");
                            }

                            var diggerName = userCommand.Parameters[0];

                            return new EmptyCommand
                            {
                                GameState = gameState,
                                DiggerName = diggerName
                            };
                        }

                    },
                    new CommandsDefinition()
                    {
                        CommandText = "scrap",
                        EntryDescription = "scrap [digger name]",
                        Description = "Destroys the digger indicated for bolts.",
                        Command = (userCommand, gameState) =>
                        {
                            if (!userCommand.Parameters.Any())
                            {
                                return new FailedMessageCommand("You will need to provide a digger name!");
                            }

                            var diggerName = userCommand.Parameters[0];

                            return new ScrapCommand
                            {
                                GameState = gameState,
                                DiggerName = diggerName
                            };
                        }
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "repair",
                        EntryDescription = "repair [digger name]",
                        Description = "After confirmation repairs the digger to max durability for the quoted price.",
                        Execute = RepairHandler()
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "inspect",
                        EntryDescription = "inspect [digger name] || inspect [digger name] lifetime || inspect [digger name] upgrades",
                        Description = "Inspect the vital statistics of the digger.",
                        Execute = InspectHandler()
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "upgrade",
                        EntryDescription = "upgrade [digger name]",
                        Description = "Begins the process to add upgrades to a digger.",
                        Execute = UpgradeHandler()
                    }
                }

            };
            return commandsGroup;
        }

        private Action<UserCommand, GameState> UpgradeHandler()
        {
            return (userCommand, gameState) =>
            {
                var scene = Scene.Create(new List<IGameEntity>
                {
                    new UpgradeHandlerEntity(gameState)
                });
                gameState.PromptText = "Enter Digger Name: ";
                Game.PushScene(scene);
                return;
            };
        }

        private Action<UserCommand, GameState> RepairHandler()
        {
            return (userCommand, gameState) =>
            {
                if (!userCommand.Parameters.Any())
                {
                    Game.WriteLine("You will need to provide a digger name!", PcmColor.Red,null,GameConsoles.Input);
                    return;
                }

                var random = new Random();

                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));
                if (digger == null)
                {
                    Game.WriteLine($"No digger named {userCommand.Parameters[0]} could be found.", PcmColor.Red,null,GameConsoles.Input);
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
                    Game.WriteLine(string.Join(Environment.NewLine, responseList.ToArray()), PcmColor.Red,null,GameConsoles.Input);
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

        private Action<UserCommand, GameState> InspectHandler()
        {
            return (userCommand, gameState) =>
            {
                if (!userCommand.Parameters.Any())
                {
                    Game.WriteLine("You will need to provide a digger name!", PcmColor.Red);
                    return;
                }

                if (!gameState.Miner.Diggers.Any(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase)))
                {
                    Game.WriteLine($"Could not find digger named {userCommand.Parameters[0]}", PcmColor.Red);
                    return;
                }

                var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                    string.Equals(x.Name, userCommand.Parameters[0], StringComparison.CurrentCultureIgnoreCase));

                if (userCommand.Parameters.Count > 1)
                {
                    if (userCommand.Parameters[1].Equals("lifetime", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ReportDiggerIdentityInfo(digger);
                        ReportDiggerLifetimeStats(digger);
                        return;
                    }

                    if (userCommand.Parameters[1].Equals("upgrades", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ReportDiggerUpgrades(digger);
                        return;
                    }

                    Game.WriteLine($"{userCommand.Parameters[1]} is not valid in this command position.");
                    return;
                }

                ReportDiggerIdentityInfo(digger);
                ReportDiggerCoreStats(digger);
                return;
            };

        }

        private static void ReportDiggerIdentityInfo(ChipDigger digger)
        {
            Game.ClearConsole();
            var headTable = new TableOutput(80, PcmColor.Yellow);
            headTable.AddHeaders("Name", "Class", "Equipped Date");
            headTable.AddRow($"{digger.Name}", $"{digger.Class.ToString()}",
                $"{digger.FirstEquipped}");
            Game.Write(headTable);
        }

        private static void ReportDiggerCoreStats(ChipDigger digger)
        {
            var vitalsTable = new TableOutput(80, PcmColor.Yellow);
            vitalsTable.AddHeaders("Stat", "Value");
            vitalsTable.AddRow("Site Hardness", digger.MineSite.Hardness.ToString());
            vitalsTable.AddRow("Site Chip Density", digger.MineSite.ChipDensity.ToString());
            vitalsTable.AddRow("Durablity (Left) / (Max)", $"{digger.Durability} / {digger.MaxDurability}");
            vitalsTable.AddRow("Hopper", digger.Hopper.Name);
            vitalsTable.AddRow("Hopper Space (Left) / (Max)",
                $"{digger.Hopper.Max - digger.Hopper.Count} / {digger.Hopper.Max}");
            Game.Write(vitalsTable);
        }

        private static void ReportDiggerLifetimeStats(ChipDigger digger)
        {
            Game.WriteLine("Lifetime Statistics",PcmColor.Black,PcmColor.Yellow);
            var lifetimeTable = new TableOutput(80, PcmColor.Yellow);
            lifetimeTable.AddHeaders("Stat", "Value");
            lifetimeTable.AddRow("First Equipped", digger.FirstEquipped.ToString(CultureInfo.InvariantCulture));
            lifetimeTable.AddRow("Lifetime Digs", digger.GetLifeTimeStat(DiggerStats.LifetimeDigs).ToString());
            lifetimeTable.AddRow("Lifetime Chips", digger.GetLifeTimeStat(DiggerStats.LifetimeChips).ToString());
            lifetimeTable.AddRow("Lifetime Repairs", digger.GetLifeTimeStat(DiggerStats.LifetimeRepairs).ToString());
            lifetimeTable.AddRow("Lifetime Bolts Cost", digger.GetLifeTimeStat(DiggerStats.LifeTimeBoltsCost).ToString());
            lifetimeTable.AddRow("Lifetime Tokes Cost", digger.GetLifeTimeStat(DiggerStats.LifeTimeTokensCost).ToString());
            Game.Write(lifetimeTable);
        }

        private static void ReportDiggerUpgrades(ChipDigger digger)
        {
            Game.WriteLine("Available Upgrades", PcmColor.Black, PcmColor.Yellow);
            var upgradesTable = new TableOutput(80, PcmColor.Yellow);
            upgradesTable.AddHeaders("Name", "Max Level", "Current Level", "Slot");
            foreach (var diggerUpgrade in digger.Upgrades)
            {
                upgradesTable.AddRow(diggerUpgrade.Name,
                    diggerUpgrade.MaxLevel.ToString(),
                    diggerUpgrade.CurrentLevel.ToString(),
                    diggerUpgrade.Slot.ToString()
                );
                upgradesTable.AddRow(diggerUpgrade.Description);
            }

            Game.Write(upgradesTable);
        }
    }
}