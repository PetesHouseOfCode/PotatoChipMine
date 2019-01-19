using PotatoChipMine.GameRooms.ControlRoom;
using PotatoChipMine.GameRooms.Store;

namespace PotatoChipMine.Models
{
    public class GameState
    {
        public Miner Miner { get; set; }
        public GameMode Mode { get; set; }
        public bool Running { get; internal set; }
        public MinerStore Store { get; internal set; }
        public DiggerControlRoom ControlRoom { get; set; }
    }
}