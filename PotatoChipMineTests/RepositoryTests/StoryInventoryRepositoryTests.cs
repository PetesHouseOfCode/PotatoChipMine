using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using System.Linq;
using PotatoChipMine.Resources;

namespace PotatoChipMineTests.RepositoryTests
{
    public class StoryInventoryRepositoryTests
    {
        const int TOTAL_NUMBER_OF_ITEMS = 2;
        StoryInventoryRepository repository;

        public StoryInventoryRepositoryTests()
        {
            repository = new StoryInventoryRepository(@"RepositoryTests\Resources\basic-storeInventory.csv");
        }

        [Fact]
        public void Load_all_storeItems_from_file()
        {
            var storeItems = repository.GetAll();

            storeItems.Count().ShouldBe(TOTAL_NUMBER_OF_ITEMS);
        }

        [Fact]
        public void Load_all_fields_of_storeItem()
        {
            var item = repository.GetAll().First();
            item.GameItemId.ShouldBe(0);
            item.BuyingPrice.ShouldBe(10);
            item.SellingPrice.ShouldBe(0);
            item.MinCount.ShouldBe(0);
            item.MaxCount.ShouldBe(0);
        }
    }
}
