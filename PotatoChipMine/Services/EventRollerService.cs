using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public class EventRollerService
    {
        private readonly GameUI _gameUi;
        private readonly GameState _gameState;
        private readonly Random rnd = new Random();
        private Thread _thread;
        private bool _paused = false;
        public EventRollerService(GameUI gameUi, GameState gameState)
        {
            _gameState = gameState;
            _gameUi = gameUi;
        }

        public void Start()
        {
            if (_thread != null && _thread.IsAlive)
                _thread.Join();
            _thread = new Thread(EventsLoop);
            _thread.Start();
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Resume()
        {
            _paused = false;
        }

        private void EventsLoop()
        {
            while (_gameState.Running)
            {
                if (!_paused)
                {
                    var x = rnd.Next(1, 9);
                    if (x == 7)
                    {
                        _gameState.NewEvents.Add(new GameEvent
                        {
                            Name = "TestEvent",
                            Description = "Adds 10 tokens",
                            Message = "You win 10 tokens in the lottery",
                            HandlerAction =
                                (gameState, gameUi) =>
                                {
                                    gameState.Miner.TaterTokens += 10;
                                    gameState.Miner.LifetimeTokens = +10;
                                }
                        });
                    }

                    if (x == 1)
                    {
                        _gameState.NewEvents.Add(new GameEvent
                        {
                            Name = "Diggers Re-Stocked",
                            Description = "Adds 5 diggers to the store stock",
                            Message = "A new shipment of diggers has arrived at the store store!",
                            HandlerAction =
                                (gameState, gameUi) =>
                                {
                                    gameState.Store.StoreState.ItemsForSale.FirstOrDefault(i =>
                                        i.Name.Equals("digger", StringComparison.CurrentCultureIgnoreCase)).Count = +5;
                                }
                        });

                    }
                }

                Thread.Sleep(20000);
            }
        }

        public void ReportEvents()
        {
            _paused = true;
            foreach (var newEvent in _gameState.NewEvents)
            {
                _gameUi.ReportEvent(newEvent.Message);
                newEvent.HandlerAction(_gameState, _gameUi);
                _gameState.EventsHistory.Add(new EventLog
                {
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    Processed = DateTime.Now.ToString()
                });
            }
            _gameState.NewEvents = new List<GameEvent>();
            _paused = false;
        }
    }
}
