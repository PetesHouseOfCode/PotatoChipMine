using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Models
{
    public class Range<T>
    {
        public T Min { get; }
        public T Max { get; }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
    }
    
    public class ChipDiggerBit
    {
        private static Random random = new Random();
        
        public string Name { get; private set; }
        public Range<int> Range { get; private set; }

        public ChipDiggerBit(string name, Range<int> range)
        {
            Name = name;
            Range = range;
        }
        
        public int Dig(decimal density)
        {
            return random.Next((int)Math.Round(Range.Min * density), (int)Math.Round(Range.Max * density));
        }

        public ChipDiggerBitState GetState()
        {
            return new ChipDiggerBitState
            {
                Name = Name,
                Min = Range.Min,
                Max = Range.Max
            };
        }

        public static ChipDiggerBit From(ChipDiggerBitState diggerBit)
        {
            return new ChipDiggerBit(diggerBit.Name, new Range<int>(diggerBit.Min, diggerBit.Max));
        }
    }
}