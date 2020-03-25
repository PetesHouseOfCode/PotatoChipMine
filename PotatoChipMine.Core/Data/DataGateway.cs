using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;
using System;

namespace PotatoChipMine.Core.Data
{
    public class DataGateway
    {
        public IRepository<IAchievementReward> Rewards { get; }
        public IRepository<GameItem> GameItems { get; }
        public IRepository<GameAchievement> GameAchievements { get; }

        public DataGateway(IRepository<IAchievementReward> rewardsRepository,
            IRepository<GameItem> gameItemsRepository,
            IRepository<GameAchievement> gameAchievementsRepository)
        {
            Rewards = rewardsRepository;
            GameItems = gameItemsRepository;
            GameAchievements = gameAchievementsRepository;
        }
    }
}
