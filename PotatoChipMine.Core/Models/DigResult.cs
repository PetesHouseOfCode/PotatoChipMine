using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.Models
{
    public class DigResult
    {
        public bool Failed { get { return FaultMessages.Any(); } }

        public int ChipsDug { get; }

        public int DurabilityLost { get; }

        public List<string> FaultMessages { get; } = new List<string>();

        private DigResult(int chipsDug, int durabilityLost, List<string> faultMessages = null)
        {
            ChipsDug = chipsDug;
            DurabilityLost = durabilityLost;
            FaultMessages = faultMessages ?? new List<string>();
        }

        public static readonly DigResult EmptyDig = new DigResult(0, 0);

        public static DigResult Success(int chipsDug, int durabilityLost)
        {
            return new DigResult(chipsDug, durabilityLost);
        }

        public static DigResult Fault(List<string> faultMessages)
        {
            return new DigResult(0, 0, faultMessages);
        }
    }
}