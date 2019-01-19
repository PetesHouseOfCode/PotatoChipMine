using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.Store.Services
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
            storeState.ItemsForSale.Add(new StoreItem {Name = "bolts", Count = 500, Price = 5,InventoryId = 2});
            storeState.ItemsBuying.Add(new StoreItem {Name = "RawChips", Price = 10});
            var store = new MinerStore(_ui, _gameState, storeState, greeting);
            return store;
        }
    }
}