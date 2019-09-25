using PotatoChipMine.Resources;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PotatoChipMineTests.RepositoryTests
{
    public class GameItemRepositoryTests
    {
        [Fact]
        public void Load_all_achievements_in_file()
        {
            var repo = new GameItemRepository(@"RepositoryTests\Resources\game-item.csv");
            var records = repo.GetAll();
            records.Count().ShouldBe(5);
        }
    }
}
