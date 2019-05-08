using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.ControlRoom.Services
{
    public class RepairHandlerEntity : GameEntity
    {
        public ChipDigger Digger { get; set; }
        public int TokenCost { get; set; }
        public int BoltsCost { get; set; }

        bool prompted;

        public RepairHandlerEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            if (command.CommandText.ToLower() == "no")
            {
                EndScene();
                return;
            }

            if (command.CommandText.ToLower() != "yes")
            {
                Game.WriteLine($"\'{command.CommandText}\' isn't a valid response.");
                return;
            }

            GameState.Miner.TaterTokens -= TokenCost;
            GameState.Miner.Inventory("bolts").Count -= (int)BoltsCost;
            Digger.Durability = Digger.MaxDurability;

            EndScene();
        }

        private void EndScene()
        {
            GameState.PromptText = null;
            Game.PopScene();
        }

        public override void Update(Frame frame)
        {
            if (prompted)
                return;

            prompted = true;
            GameState.PromptText = "Do you want to perform repairs? ";
            Game.WriteLine($"Repairs will cost {TokenCost} tater tokens and {BoltsCost} bolts.", PcmColor.Green);
        }
    }
}