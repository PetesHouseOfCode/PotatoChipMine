using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.Models;
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
                        Command = (userCommand, gameState) =>
                        {
                            if (!userCommand.Parameters.Any())
                            {
                                return new FailedMessageCommand("You will need to provide a digger name!");
                            }

                            var diggerName = userCommand.Parameters[0];

                            return new RepairCommand
                            {
                                GameState = gameState,
                                DiggerName = diggerName
                            };
                        }
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "inspect",
                        EntryDescription = "inspect [digger name] || inspect [digger name] lifetime || inspect [digger name] upgrades",
                        Description = "Inspect the vital statistics of the digger.",
                        Command = (userCommand, gameState) =>
                        {
                            if (!userCommand.Parameters.Any())
                            {
                                return new FailedMessageCommand("You will need to provide a digger name!");
                            }

                            var diggerName = userCommand.Parameters[0];

                            var inspectType = InspectCommandTypes.Default;
                            if(userCommand.Parameters.Count > 1)
                            {
                                var param = userCommand.Parameters[1];
                                if(string.Equals(param, "upgrades", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    inspectType = InspectCommandTypes.Upgrades;
                                }
                                else if(string.Equals(param, "lifetime", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    inspectType = InspectCommandTypes.LifeTime;
                                }
                            }

                            return new InspectCommand
                            {
                                GameState = gameState,
                                DiggerName = diggerName,
                                InspectType = inspectType
                            };
                        }
                    },
                    new CommandsDefinition()
                    {
                        CommandText = "upgrade",
                        EntryDescription = "upgrade [digger name]",
                        Description = "Begins the process to add upgrades to a digger.",
                        Command = (userCommand, gameState) =>
                        {
                            return new UpgradeCommand{GameState = gameState};
                        }
                    }
                }
            };

            return commandsGroup;
        }
    }
}