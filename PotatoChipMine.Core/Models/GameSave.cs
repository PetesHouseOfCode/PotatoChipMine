using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Services.PersistenceService;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Models
{
    public class GameSave
    {
        public MinerState Miner { get; set; }
        public StoreInventoryState MinerStore { get; set; }
        public GameMode Mode { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; }
        public List<ClaimListingState> ClaimListings { get; set; } = new List<ClaimListingState>();
    }

    public class ClaimListingState
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int LeasePrice { get; set; }
        public MineClaimState Claim { get; set; }
        public SurveyResultState Survey { get; set; }
    }

    public class SurveyResultState
    {
        public string Hardness { get; set; }
        public string Density { get; set; }
    }
}