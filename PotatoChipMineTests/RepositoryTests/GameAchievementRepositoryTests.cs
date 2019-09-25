using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Resources;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PotatoChipMineTests.RepositoryTests
{
    public class GameAchievementFileLoad_Tests
    {
        const int INVENTORY_ACHIEVEMENT_ID = 1;
        const int TOTAL_NUMBER_OF_ACHIEVEMENTS = 2;
        const int LIFETIMESTAT_ACHIEVEMENT_ID = 2;
        const int INV_ACHIEVEMENT_MINAMOUNT = 1;
        const int LIFETIME_ACHIEVEMENT_MINCOUNT = 1;

        readonly AchievementRepository achievementRepo;

        public GameAchievementFileLoad_Tests()
        {
            var resourcePath = @"RepositoryTests\Resources\basic-achievements.csv";
            var gameState = new GameState();
            achievementRepo = new AchievementRepository(resourcePath, gameState);
        }

        [Fact]
        public void Load_all_achievements_in_file()
        {
            var achievements = achievementRepo.GetAll();
            achievements.Count().ShouldBe(TOTAL_NUMBER_OF_ACHIEVEMENTS);
        }

        [Fact]
        public void Load_inventory_achievement_data_when_that_type()
        {
            var achievements = achievementRepo.GetAll();
            var achievement = achievements.First(x=>x.Id == INVENTORY_ACHIEVEMENT_ID);

            achievement.ShouldBeOfType<InventoryAchievement>();
            var achievementSetting = (InventoryAchievementSetting)achievement.GetSetting();
            achievementSetting.Name.ShouldBe("Achievement1Name");
            achievementSetting.Description.ShouldBe("Achievement1Description");
            achievementSetting.InventoryItemName.ShouldBe("Achievement1ItemName");
            achievementSetting.MinAmount.ShouldBe(INV_ACHIEVEMENT_MINAMOUNT);
        }

        [Fact]
        public void Load_lifetimeStat_achievement_data_when_that_type()
        {
            var achievements = achievementRepo.GetAll();
            var achievement = achievements.First(x => x.Id == LIFETIMESTAT_ACHIEVEMENT_ID);

            achievement.ShouldBeOfType<LifetimeStatAchievement>();
            var achievementSetting = (LifetimeStatAchievementSetting)achievement.GetSetting();
            achievementSetting.Name.ShouldBe("Achievement2Name");
            achievementSetting.Description.ShouldBe("Achievement2Description");
            achievementSetting.LifetimeStatName.ShouldBe("Achievement2StatName");
            achievementSetting.MinCount.ShouldBe(LIFETIME_ACHIEVEMENT_MINCOUNT);
        }
    }
}
