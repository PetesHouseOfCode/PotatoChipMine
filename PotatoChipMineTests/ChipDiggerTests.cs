using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using Shouldly;
using Xunit;

namespace PotatoChipMineTests
{
    public class ChipDiggerTests
    {
        [Fact]
        public void ChipDiggerDigTest()
        {
            var chipDigger = new ChipDigger();
            var scoop = chipDigger.Dig();
            scoop.Chips.ShouldBeGreaterThan(0);
        }
    }

    public class ChipDigger
    {
        public Scoop Dig()
        {
            return new Scoop(){Chips = 1};
        }
    }

    public class Scoop
    {
        public int Chips { get; set; }
    }
}
