using PotatoChipMine.Services;
using PotatoChipMine.Store.Models;

namespace PotatoChipMine.Store.Services
{
    public class MinerStoreFactory
    {
        private readonly GameState _gameState;
        private readonly GameUI _ui;

        public MinerStoreFactory(GameUI ui, GameState gameState)
        {
            _gameState = gameState;
            _ui = ui;
        }

        public MinerStore BuildMineStore()
        {
            var greeting = new[]
            {
                "Welcome to the miners store.", "You can type [help] for a list of the store commands.",
                "To leave the store type exit."
            };

            var storeState = new StoreState();
            storeState.ItemsForSale.Add(new StoreItem {Name = "Digger", Price = 20, Count = 5, InventoryId = 1});
            storeState.ItemsBuying.Add(new StoreItem {Name = "RawChips", Price = 10});
            var store = new MinerStore(_ui, _gameState, storeState, greeting);
            return store;
        }
    }
}