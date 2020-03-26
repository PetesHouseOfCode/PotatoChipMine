using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using System.Linq;

namespace PotatoChipMine.Core.GameRooms.Store.Services
{
    public class MinerStoreFactory
    {
        private readonly GameState _gameState;
        private readonly CommandsGroup _baseCommandsGroup;
        readonly DataGateway _gateway;

        public MinerStoreFactory(GameState gameState, CommandsGroup baseCommandsGroup, DataGateway gateway)
        {
            _gateway = gateway;
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
            var storeState = new StoreInventory();
            // Add Standard_Digger
            storeState.ItemsForSale.Add(new StoreItem
            {
                Price = 20,
                Count = 5,
                Item = _gateway.GameItems.GetAll().First(gi => gi.Id == 1)
            });
            // Add Bolts
            storeState.ItemsForSale.Add(new StoreItem
            {
                Count = 500,
                Price = 5,
                Item = _gateway.GameItems.GetAll().First(gi => gi.Id == 2)
            });

            // Add RawChips
            storeState.ItemsBuying.Add(new StoreItem
            {
                Price = 10,
                Item = _gateway.GameItems.GetAll().First(gi => gi.Id == 3)
            });
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