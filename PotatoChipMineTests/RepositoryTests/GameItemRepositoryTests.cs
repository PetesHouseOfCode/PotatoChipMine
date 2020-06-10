using PotatoChipMine.Core.Services;
using PotatoChipMine.Resources;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace PotatoChipMineTests.RepositoryTests
{
    public class GameItemRepositoryTests
    {
        const int TOTAL_NUMBER_OF_ITEMS = 3;
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
            var gameItem = gameItemRepo.GetAll().First(x => x.Id == 1);
            gameItem.Name.ShouldBe("Name1");
            gameItem.PluralizedName.ShouldBe("PluralizedName1");
            gameItem.Description.ShouldBe("Description1");
        }

        [Fact]
        public void Load_gameItem_of_ChipsHopperUpgradeItem()
        {
            var gameItem = gameItemRepo.GetAll().First(x => x.Id == 2) as ChipsHopperUpgradeItem;
            gameItem.Name.ShouldBe("Name2");
            gameItem.PluralizedName.ShouldBe("PluralizedName2");
            gameItem.RequiredSlotLevel.ShouldBe(2);
            gameItem.Level.ShouldBe(1);
            gameItem.Size.ShouldBe(100);
        }

        [Fact]
        public void Load_gameItem_of_BitUpgradeItem()
        {
            var gameItem = gameItemRepo.GetAll().First(x => x.Id == 3) as BitUpgradeItem;
            gameItem.Name.ShouldBe("Name3");
            gameItem.PluralizedName.ShouldBe("PluralizedName3");
            gameItem.RequiredSlotLevel.ShouldBe(2);
            gameItem.Level.ShouldBe(1);
            gameItem.Min.ShouldBe(1);
            gameItem.Max.ShouldBe(2);
        }

        [Fact]
        public void Loading_datafile_with_invalid_game_type_Fails()
        {
            var repo = new GameItemRepository(@"RepositoryTests\Resources\basic-gameItems-invalid-itemtype.csv");
            Should.Throw<InvalidDataException>(() => repo.GetAll());
        }
    }
}
