using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Models
{
    public class MineClaim
    {
        public static MineClaim Default = new MineClaim(ChipDensity.Rich, SiteHardness.Hard);

        public int Id { get; private set; }
        public ChipDensity ChipDensity { get; }
        public SiteHardness Hardness { get; }

        public MineClaim(ChipDensity chipDensity, SiteHardness siteHardness) :
            this(0, chipDensity, siteHardness)
        {
        }

        private MineClaim(int id, ChipDensity chipDensity, SiteHardness siteHardness)
        {
            Id = id;
            ChipDensity = chipDensity;
            Hardness = siteHardness;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public MineClaimState GetState()
        {
            return new MineClaimState
            {
                Id = Id,
                ChipDensity = ChipDensity,
                Hardness = Hardness
            };
        }

        public static MineClaim FromState(MineClaimState state)
        {
            return new MineClaim(
                state.Id,
                state.ChipDensity,
                state.Hardness);
        }
    }

    public enum SiteHardness
    {
        Soft = 1,
        Firm = 2,
        Hard = 3,
        Solid = 4
    }

    public class MineClaims
    {
        private List<MineClaim> mineClaims = new List<MineClaim>();

        public IReadOnlyList<MineClaim> GetAll()
        {
            return mineClaims;
        }

        public MineClaim Add(MineClaim mineClaim)
        {
            for (var i = 1; true; i++)
            {
                if (mineClaims.All(x => x.Id != i))
                {
                    mineClaim.SetId(i);
                    break;
                }
            }

            mineClaims.Add(mineClaim);
            return mineClaim;
        }

        public bool HasId(int claimLeaseId)
        {
            return mineClaims.Any(x => x.Id == claimLeaseId);
        }
    }
}