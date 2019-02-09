using System;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class LotteryEvent : GameComponent, IGameComponent
    {
        private readonly Random _rnd = new Random();

        private TimeSpan _lastLotteryTime = TimeSpan.Zero;

        public LotteryEvent(GameState gameState)
            : base(gameState)
        {
        }

        public override void Update(Frame frame)
        {
            if (_lastLotteryTime != TimeSpan.Zero && frame.TimeSinceStart.Subtract(_lastLotteryTime).TotalSeconds < 5)
            {
                return;
            }

            _lastLotteryTime = frame.TimeSinceStart;

            var x = _rnd.Next(1, 9);
            if (x == 7)
            {
                GameState.Miner.TaterTokens += 10;
                GameState.Miner.LifetimeTokens = +10;

                GameState.NewEvents.Add(new GameEvent
                {
                    Name = "TestEvent",
                    Description = "Adds 10 tokens",
                    Message = "You win 10 tokens in the lottery"
                });
            }
        }
    }
}
