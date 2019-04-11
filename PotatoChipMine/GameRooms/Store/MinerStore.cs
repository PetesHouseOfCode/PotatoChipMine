using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.GameRooms.Store.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.Store
{
    public class MinerStore : GameRoom
    {
        public MinerStore(
            GameState gameState,
            string[] greeting,
            CommandsGroup commandsGroup)
            : base(gameState, greeting, GameMode.Store)
        {
            CommandsGroup = commandsGroup;
            this.Name = "store";
        }

        public StoreState StoreState { get; set; }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.Store;
            base.EnterRoom();
        }
    }
}