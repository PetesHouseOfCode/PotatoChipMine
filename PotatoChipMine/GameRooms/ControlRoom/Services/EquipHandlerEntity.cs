using System;
using System.Linq;
using PotatoChipMine.GameEngine;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom.Services
{
    public class EquipHandlerEntity : GameEntity
    {
        public EquipHandlerEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            if (string.IsNullOrEmpty(command.CommandText))
            {
                Game.WriteLine("A name is required!", ConsoleColor.Red);
                return;
            }
            
            var newDiggerName = command.CommandText.Trim().Replace(" ", "-");

            if (GameState.Miner.Diggers.Exists(x => x.Name == newDiggerName))
            {
                Game.WriteLine($"Digger with the name {newDiggerName} already exists.", ConsoleColor.Red);
                return;
            }
            
            var digger = GameState.Miner.InventoryItems.FirstOrDefault(x => x.Name.ToLower() == "digger");
            var factory = new MineSiteFactory();
            var newDigger = new ChipDigger(factory.BuildSite()) { Durability = 20 };
            newDigger.Name = newDiggerName;
            digger.Count--;

            Game.Write($"Digger {newDigger.Name} has been equipped on ");
            Game.Write($"{newDigger.MineSite.ChipDensity.ToString()} density", ConsoleColor.Blue);
            Game.Write(" with a ");
            Game.Write($"{newDigger.MineSite.Hardness.ToString()} hardness", ConsoleColor.Cyan);
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