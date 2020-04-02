using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Data
{
    public class DataGateway
    {
        public IRepository<IAchievementReward> Rewards { get; }
        public IRepository<GameItem> GameItems { get; }
        public IRepository<GameAchievement> GameAchievements { get; }
        public IRepository<StoreItem> StoreItems {get;}

        public DataGateway(IRepository<IAchievementReward> rewardsRepository,
            IRepository<GameItem> gameItemsRepository,
            IRepository<GameAchievement> gameAchievementsRepository,
            IRepository<StoreItem> storeInventoryRepository)
        {
            StoreItems = storeInventoryRepository;
            Rewards = rewardsRepository;
            GameItems = gameItemsRepository;
            GameAchievements = gameAchievementsRepository;
        }
    }
}
