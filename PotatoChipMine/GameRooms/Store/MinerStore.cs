using PotatoChipMine.GameRooms.Store.Models;
using PotatoChipMine.GameRooms.Store.Services;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.Store
{
    public class MinerStore : GameRoom
    {
        public StoreState StoreState { get; set; }

        public MinerStore(GameUI ui, GameState gameState, StoreState storeState, string[] greeting)
            : base(ui, gameState, greeting, GameMode.Store)
        {
            StoreState = storeState;
            CommandsGroup = new StoreCommandsGroupFactory(ui, gameState, StoreState).Build();
            this.Name = "store";
        }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.Store;
            base.EnterRoom();
        }

        public override void ExecuteCommand(UserCommand command)
        {
            CommandsGroup.ExecuteCommand(Ui, command, GameState);
        }
    }
}