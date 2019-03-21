using System.Linq;
using PotatoChipMine.GameEngine;
using PotatoChipMine.Models;
using PotatoChipMine.Services;

namespace PotatoChipMine.GameRooms.ControlRoom.Services
{
    public class EquipHandlerEntity : GameEntity
    {
        public EquipHandlerEntity(GameState gameState)
            : base(gameState)
        {
        }

        public override void HandleInput(UserCommand command)
        {
            var digger = GameState.Miner.InventoryItems.FirstOrDefault(x => x.Name.ToLower() == "digger");
            var factory = new MineSiteFactory();
            var newDigger = new ChipDigger(factory.BuildSite()) { Durability = 20 };
            newDigger.Name = command.CommandText.Trim().Replace(" ", "-");
            digger.Count--;
            GameState.Miner.Diggers.Add(newDigger);
            
            GameState.NewEvents.Add(new GameEvent
            {
                Name = "DiggerEquipped",
                Description = "Equips a digger from your inventory",
                Message = $"Digger {newDigger.Name} has been equipped"
            });

            GameState.PromptText = null;
            Game.PopScene();
        }

        public override void Update(Frame frame)
        {
        }
    }
}