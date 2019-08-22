using System;
using System.Linq;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;

namespace PotatoChipMine.Core.GameRooms.ControlRoom.Services
{
    public class UpgradeHandlerEntity : GameEntity
    {
        private ChipDigger digger;
        private InventoryItem item;
        private int dialogStep = 1;

        public UpgradeHandlerEntity(GameState gameState) : base(gameState)
        {
            
        }

        public override void HandleInput(UserCommand command)
        {
            switch (dialogStep)
            {
                case 1:
                    if (string.IsNullOrEmpty(command.FullCommand))
                    {
                        Game.WriteLine("A name is required!", PcmColor.Red,null,GameConsoles.Input);
                        return;
                    }

                    if (!GameState.Miner.Diggers.Any(x =>
                        string.Equals(x.Name, command.FullCommand, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        Game.WriteLine($"There is no digger named {command.CommandText}", PcmColor.Red, null,
                            GameConsoles.Input);
                        return;
                    }

                    digger = GameState.Miner.Diggers.First(x =>
                        string.Equals(x.Name, command.FullCommand, StringComparison.InvariantCultureIgnoreCase));
                    GameState.PromptText = "Enter upgrade item to use:";
                    dialogStep++;
                    break;
                case 2:
                    if (string.IsNullOrEmpty(command.FullCommand))
                    {
                        Game.WriteLine("An upgrade item is required!", PcmColor.Red, null, GameConsoles.Input);
                        return;
                    }

                    item = GameState.Miner.InventoryItems.FirstOrDefault(x =>
                        string.Equals(x.Item.Name, command.FullCommand, StringComparison.CurrentCultureIgnoreCase));
                    if (item == null || item.Count < 1)
                    {
                        Game.WriteLine($"You don't have any {command.FullCommand}!");
                        return;
                    }
                    
                    if(!(item.Item is DiggerUpgradeItem))
                    {
                        Game.WriteLine($"The item is not a digger upgrade!");
                        return;
                    }
                    
                    var result = DiggerUpgrader.ApplyUpgrade(digger, item.Item as DiggerUpgradeItem);
                    if (!result.completed)
                    {
                        Game.WriteLine(result.message,PcmColor.Red,null,GameConsoles.Input);
                    }

                    GameState.Miner.InventoryItems.FirstOrDefault(x =>
                        string.Equals(x.Item.Name, item.Item.Name, StringComparison.InvariantCultureIgnoreCase)).Count--;
                    GameState.PromptText = null;
                    Game.WriteLine($"{digger.Name} has been upgraded. {item.Item.Name}");
                    Game.WriteLine(item.Item.Description);
                    Game.PopScene();
                    break;                    
            }
        }
    }
}