using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
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
            foreach (var digger in diggers.Where(x => x.ReadyToDig(frame.TimeSinceStart)))
            {
                var digResult = digger.Dig(frame.TimeSinceStart);
                if (!digResult.Failed)
                {
                    var table = new TableOutput(80, PcmColor.DarkYellow);
                    table.AddHeaders("Name", "Dug", "Damage", "Durability", "Hopper");
                    table.AddRow(
                        digger.Name,
                        digResult.ChipsDug.ToString(),
                        digResult.DurabilityLost.ToString(),
                        $"{digger.Durability.Current}/{digger.Durability.Max}",
                        $"{digger.Hopper.Count}/{digger.Hopper.Max}");

                    Game.Write(table, GameConsoles.Events);
                }
                else
                {
                    foreach (var message in digResult.FaultMessages)
                        Game.WriteLine(
                            message,
                            PcmColor.Cyan,
                            PcmColor.Black,
                            GameConsoles.Events);
                }
            }
        }
    }
}