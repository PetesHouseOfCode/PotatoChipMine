using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.ControlRoom.Services
{
    public class EquipHandlerEntity : GameEntity
    {
        public EquipHandlerEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            if (string.IsNullOrEmpty(command.FullCommand))
            {
                Game.WriteLine("A name is required!", PcmColor.Red);
                return;
            }
            
            var newDiggerName = command.FullCommand.Trim().Replace(" ", "-");

            if (GameState.Miner.Diggers.Exists(x => x.Name == newDiggerName))
            {
                Game.WriteLine($"Digger with the name {newDiggerName} already exists.", PcmColor.Red);
                return;
            }
            
            var digger = GameState.Miner.InventoryItems.FirstOrDefault(x => x.Name.ToLower() == "digger");
            var factory = new MineSiteFactory();
            var newDigger = new ChipDigger(factory.BuildSite()) { Durability = 20 };
            newDigger.Name = newDiggerName;
            digger.Count--;

            Game.Write($"Digger {newDigger.Name} has been equipped on ");
            Game.Write($"{newDigger.MineSite.ChipDensity.ToString()} density", PcmColor.Blue);
            Game.Write(" with a ");
            Game.Write($"{newDigger.MineSite.Hardness.ToString()} hardness", PcmColor.Cyan);
            Game.WriteLine("");
            GameState.Miner.Diggers.Add(newDigger);
            
            GameState.PromptText = null;
            Game.PopScene();
        }

        public override void Update(Frame frame)
        {
        }
    }
}