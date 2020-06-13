using PotatoChipMine.Core.Models;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Models.Claims
{
    public class SurveyResults
    {
        public string Density { get; }
        public string Hardness { get; }

        public SurveyResults(string density, string hardness)
        {
            Density = density;
            Hardness = hardness;
        }

        public static SurveyResults NoSurvey()
        {
            return new SurveyResults("Unknown", "Unknown");
        }

        public static SurveyResults GetFromClaim(MineClaim claim)
        {
            return new SurveyResults(claim.ChipDensity.ToString(), claim.Hardness.ToString());
        }

        public SurveyResultState GetState()
        {
            return new SurveyResultState
            {
                Density = Density,
                Hardness = Hardness
            };
        }

        public static SurveyResults FromState(SurveyResultState state)
        {
            return new SurveyResults(state.Density, state.Hardness);
        }
    }
}
