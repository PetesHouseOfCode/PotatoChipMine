using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Events
{
    public class LotteryEvent : GameEntity, IGameEntity
    {
        private const int WinningNumberOfTokens = 10;
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
                GameState.Miner.TaterTokens += WinningNumberOfTokens;
                GameState.Miner.UpdateLifetimeStat(Stats.LifetimeTokens, WinningNumberOfTokens);

                GameState.NewEvents.Add(new GameEvent
                {
                    Name = "TestEvent",
                    Description = $"Adds {WinningNumberOfTokens} tokens",
                    Message = $"You win {WinningNumberOfTokens} tokens in the lottery"
                });
            }
        }
    }
}
