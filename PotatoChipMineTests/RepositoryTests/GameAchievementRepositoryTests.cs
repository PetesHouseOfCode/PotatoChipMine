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
    public class GameAchievementRepositoryTests
    {
        [Fact]
        public void nothing()
        {
            var resourcePath = @"RepositoryTests\Resources\achievements.csv";
            var gameState = new GameState();

            var repo = new AchievementRepository(resourcePath, gameState);
            var achievements = repo.GetAll();
            achievements.Count().ShouldBe(3);
        }
    }
}
