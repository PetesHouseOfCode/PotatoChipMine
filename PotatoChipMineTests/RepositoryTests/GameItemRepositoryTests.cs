using PotatoChipMine.Resources;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace PotatoChipMineTests.RepositoryTests
{
    public class GameItemRepositoryTests
    {
        const int TOTAL_NUMBER_OF_ITEMS = 2;
        GameItemRepository gameItemRepo = new GameItemRepository(@"RepositoryTests\Resources\basic-gameItems.csv");

        [Fact]
        public void Load_all_gameItems_in_file()
        {
            var records = gameItemRepo.GetAll();
            records.Count().ShouldBe(TOTAL_NUMBER_OF_ITEMS);
        }

        [Fact]
        public void Load_gameItem_with_complete_detail()
        {
            var gameItem = gameItemRepo.GetAll().First(x => x.ItemId == 1);
            gameItem.Name.ShouldBe("Name1");
            gameItem.Description.ShouldBe("Description1");
        }
    }
}
