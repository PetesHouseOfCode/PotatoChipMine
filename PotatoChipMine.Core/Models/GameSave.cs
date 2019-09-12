using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Services.PersistenceService;

namespace PotatoChipMine.Core.Models
{
    public class GameSave
    {
        public MinerState Miner { get; set; }
        public StoreInventoryState MinerStore { get; set; }
        public GameMode Mode { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; }
    }
}