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
            : base(ui, gameState, greeting,GameMode.Store)
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
            EventRollerService.ReportEvents();
            EventRollerService.Pause();
            var commands = Ui.AcceptUserCommand("Store");
            EventRollerService.Resume();
            foreach(var command in commands)
                CommandsGroup.ExecuteCommand(Ui, command, GameState);
        }
    }
}