using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Commands
{
    public class RepairCommand : CommandWithGameState, ICommand
    {
        public string DiggerName { get; set; }
    }

    public class RepairCommandHandler : ICommandHandler<RepairCommand>
    {
        public void Handle(RepairCommand command)
        {
            var gameState = command.GameState;

            var random = new Random();

            var digger = gameState.Miner.Diggers.FirstOrDefault(x =>
                string.Equals(x.Name, command.DiggerName, StringComparison.CurrentCultureIgnoreCase));
            if (digger == null)
            {
                Game.WriteLine($"No digger named {command.DiggerName} could be found.", PcmColor.Red, null, GameConsoles.Input);
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
                Game.WriteLine(string.Join(Environment.NewLine, responseList.ToArray()), PcmColor.Red, null, GameConsoles.Input);
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
        }
    }
}
