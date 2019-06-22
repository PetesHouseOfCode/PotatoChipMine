using PotatoChipMine.Core.GameRooms.Store.Models;

namespace PotatoChipMine.Core.Models
{
    public class GameSave
    {
        public Miner Miner { get; set; }
        public StoreState MinerStore { get; set; }
        public GameMode Mode { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; }
    }
}