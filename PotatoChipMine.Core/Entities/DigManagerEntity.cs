using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Entities
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
                var table = new TableOutput(80, PcmColor.DarkYellow);
                table.AddHeaders("Name", "Dug", "Damage", "Durability", "Hopper");
                table.AddRow(
                    digger.Name,
                    digResult.ChipsDug.ToString(),
                    digResult.DurabilityLost.ToString(),
                    $"{digger.Durability}/{digger.MaxDurability}",
                    $"{digger.Hopper.Count}/{digger.Hopper.Max}");

                Game.Write(table,GameConsoles.Events);
                
                if (digger.Hopper.IsFull)
                {
                    Game.WriteLine(
                        $"{digger.Name}--The digger hopper is full.",
                        PcmColor.Black,
                        PcmColor.DarkYellow,
                        GameConsoles.Events);
                }

                if (digger.Durability < 1)
                {
                    Game.WriteLine(
                        $"{digger.Name}--The digger needs repair!",
                        PcmColor.Black,
                        PcmColor.Red,
                        GameConsoles.Events);
                }
            }
        }
    }
}