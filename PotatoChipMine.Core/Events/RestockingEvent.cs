using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Events
{
    public class RestockingEvent : GameEntity, IGameEntity
    {
        private readonly Random _rnd = new Random();

        private TimeSpan _lastRestockEvent = TimeSpan.Zero;

        public RestockingEvent(GameState gameState)
            : base(gameState)
        {
        }

        public override void Update(Frame frame)
        {
            if (_lastRestockEvent != TimeSpan.Zero && frame.TimeSinceStart.Subtract(_lastRestockEvent).TotalSeconds < 15)
            {
                return;
            }

            _lastRestockEvent = frame.TimeSinceStart;

            var x = _rnd.Next(1, 9);
            if (x == 1)
            {
                GameState
                    .Store
                    .StoreState
                    .ItemsForSale
                    .FirstOrDefault(i =>
                        i.Name.Equals("digger", StringComparison.CurrentCultureIgnoreCase)
                        ).Count = +5;

                GameState.NewEvents.Add(new GameEvent
                {
                    Name = "Diggers Re-Stocked",
                    Description = "Adds 5 diggers to the store stock",
                    Message = "A new shipment of diggers has arrived at the store store!"
                });
            }
        }
    }
}
