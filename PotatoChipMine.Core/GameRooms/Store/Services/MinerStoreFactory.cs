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

        public MinerStore Build()
        {
            var greeting = new[]
            {
                "Welcome to the miners store.", "You can type [help] for a list of the store commands.",
                "To leave the store type exit."
            };
            var storeState = new StoreInventory();
            var storeItems = _gateway.StoreItems.GetAll();

            foreach (var item in storeItems)
            {
                if (item.BuyingPrice == 0)
                {
                    storeState.ItemsForSale.Add(new StoreItem
                    {
                        Price = item.SellingPrice,
                        Count = item.MinCount,
                        Item = _gateway.GameItems.GetAll().First(gi => gi.Id == item.GameItemId)
                    });
                }
                else
                {
                    storeState.ItemsBuying.Add(new StoreItem
                    {
                        Price = item.BuyingPrice,
                        Item = _gateway.GameItems.GetAll().First(gi => gi.Id == item.GameItemId)
                    });
                }
            }

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