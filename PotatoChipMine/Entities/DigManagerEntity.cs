using System;
using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.GameEngine;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.Entities
{
    public class DigManagerEntity : GameEntity
    {
        private readonly List<ChipDigger> diggers;
        
        public DigManagerEntity(GameState gameState)
            : base(gameState)
        {
            diggers = gameState.Miner.Diggers;
        }

        public override void HandleInput(UserCommand command)
        {
        }

        public override void Update(Frame frame)
        {
            foreach (var digger in diggers.Where(x => x.CanDig(frame.TimeSinceStart)))
            {
                var digResult = digger.Dig(frame.TimeSinceStart);
                var table = new TableOutput(100, ConsoleColor.DarkYellow);
                table.AddHeaders("Name", "Chips Dug", "Durability Lost", "Durability", "Hopper");
                table.AddRow(
                    digger.Name,
                    digResult.ChipsDug.ToString(),
                    digResult.DurabilityLost.ToString(),
                    $"{digger.Durability}/{digger.MaxDurability}",
                    $"{digger.Hopper.Count}/{digger.Hopper.Max}");

                Game.Write(table);
                
                if (digger.Hopper.IsFull)
                {
                    Game.WriteLine(
                        $"{digger.Name}--The digger hopper is full.",
                        ConsoleColor.Black,
                        ConsoleColor.DarkYellow);
                }

                if (digger.Durability < 1)
                {
                    Game.WriteLine(
                        $"{digger.Name}--The digger needs repair!",
                        ConsoleColor.Black,
                        ConsoleColor.Red);
                }
            }
        }
    }
}