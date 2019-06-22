using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;

namespace PotatoChipMine.Core.GameRooms.Store.Services
{
    public class MinerStoreFactory
    {
        private readonly GameState _gameState;
        private readonly CommandsGroup _baseCommandsGroup;

        public MinerStoreFactory(GameState gameState, CommandsGroup baseCommandsGroup)
        {
            _gameState = gameState;
            _baseCommandsGroup = baseCommandsGroup;
        }

        public MinerStore BuildMineStore()
        {
            var greeting = new[]
            {
                "Welcome to the miners store.", "You can type [help] for a list of the store commands.",
                "To leave the store type exit."
            };

            var storeState = new StoreState();
            storeState.ItemsForSale.Add(new StoreItem { Name = "Digger", Price = 20, Count = 5, InventoryId = 1 });
            storeState.ItemsForSale.Add(new StoreItem { Name = "bolts", Count = 500, Price = 5, InventoryId = 2 });
            storeState.ItemsBuying.Add(new StoreItem { Name = "RawChips", Price = 10 });
            var store = new MinerStore(
                _gameState,
                greeting,
                _baseCommandsGroup.Join(new StoreCommandsGroupFactory(_gameState, storeState).Build())
                );
            store.StoreState = storeState;
            return store;
        }
    }
}