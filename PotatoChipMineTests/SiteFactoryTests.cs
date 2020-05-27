using PotatoChipMine.Core.Services;
using Shouldly;
using Xunit;

namespace PotatoChipMineTests
{
    public partial class SiteFactoryTests
    {
        [Fact]
        public void SiteFactory_BuildSite_ReturnsNewSite()
        {
            var siteFactory = new MineClaimFactory();
            var site = siteFactory.BuildSite();

        }

        [Fact]
        public void SiteFactory_BuildSite_ReturnsNewSiteDenistyNotNull()
        {
            var siteFactory = new MineClaimFactory();
            var site = siteFactory.BuildSite();
            var densityInt = (int) site.ChipDensity;
            var hardnessInt = (int) site.Hardness;
            densityInt.ShouldBeGreaterThanOrEqualTo(1);
            densityInt.ShouldBeLessThanOrEqualTo(3);
            hardnessInt.ShouldBeGreaterThan(0);
            hardnessInt.ShouldBeLessThanOrEqualTo(4);
        }

    }

}
