using PotatoChipMine.Core.Commands;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameRooms.Store
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
            CommandRunner.Run(new StockCommand() { State = StoreState });
            base.EnterRoom();
        }
    }
}