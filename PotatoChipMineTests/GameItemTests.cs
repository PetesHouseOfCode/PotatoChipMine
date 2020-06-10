using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using PotatoChipMine.Core.Models;

namespace PotatoChipMineTests
{
    public class GameItemTests
    {
        GameItem gameItem = new GameItem
        {
            Id = 1,
            Name = "Singular",
            PluralizedName = "Plural",
            Description = ""
        };

        [Fact]
        public void GameItem_name_with_0_count_should_be_plural()
        {
            gameItem.GetNameFormBasedOnCount(0).ShouldBe(gameItem.PluralizedName);
        }

        [Fact]
        public void GameItem_name_with_1_count_should_be_singular()
        {
            gameItem.GetNameFormBasedOnCount(1).ShouldBe(gameItem.Name);
        }

        [Fact]
        public void GameItem_name_with_2_count_should_be_plural()
        {
            gameItem.GetNameFormBasedOnCount(2).ShouldBe(gameItem.PluralizedName);
        }
    }
}
