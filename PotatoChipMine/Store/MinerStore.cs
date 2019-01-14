using PotatoChipMine.Models;
using PotatoChipMine.Services;
using PotatoChipMine.Store.Models;
using PotatoChipMine.Store.Services;

namespace PotatoChipMine.Store
{
    public class MinerStore : GameRoom
    {
        protected readonly StoreState StoreState;

        public MinerStore(GameUI ui, GameState gameState, StoreState storeState, string[] greeting)
            : base(ui, gameState, greeting, GameMode.Store)
        {
            StoreState = storeState;
            CommandsGroup = new StoreCommandsGroupFactory(ui, gameState, StoreState).Build();
        }

        public override void EnterRoom()
        {
            GameState.Mode = GameMode.Store;
            base.EnterRoom();
        }

        protected override void AcceptCommand()
        {
            var command = Ui.AcceptUserCommand("Store");
            CommandsGroup.ExecuteCommand(Ui, command, GameState);
        }
    }
}