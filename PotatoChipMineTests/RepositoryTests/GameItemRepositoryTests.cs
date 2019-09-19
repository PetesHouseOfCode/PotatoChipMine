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
        public void Loading_File_Returns_Records()
        {
            var repo = new GameItemRepository(@"RepositoryTests\Resources\game-item.csv");
            var records = repo.GetAll();
            records.Count().ShouldBe(5);
        }
    }
}
