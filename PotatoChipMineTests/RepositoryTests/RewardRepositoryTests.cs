using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Resources;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace PotatoChipMineTests.RepositoryTests
{
    public class RewardRepositoryTests
    {
        const int TOTAL_NUMBER_OF_ITEMS = 2;
        RewardRepository rewardRepo = new RewardRepository(@"RepositoryTests\Resources\basic-rewards.csv");

        [Fact]
        public void Load_all_rewards_from_file()
        {
            var rewards = rewardRepo.GetAll();

            rewards.Count().ShouldBe(TOTAL_NUMBER_OF_ITEMS);
            var firstReward = rewards.First(x => x.Id == 1);
            firstReward.ShouldBeOfType<NewStoreItemReward>();
            firstReward.Id.ShouldBe(1);
            ((NewStoreItemReward)firstReward).Count.ShouldBe(10);
            ((NewStoreItemReward)firstReward).Price.ShouldBe(100);
        }

        [Fact]
        public void Reward_with_bad_type_throws_exception()
        {
            var rewardRepo = new RewardRepository(@"RepositoryTests\Resources\basic-rewards-bad-type.csv");
            Should.Throw<InvalidDataException>(() => rewardRepo.GetAll());
        }
    }
}
