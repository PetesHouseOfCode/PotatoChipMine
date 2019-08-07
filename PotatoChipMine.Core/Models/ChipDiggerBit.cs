using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Models
{
    public class ChipDiggerBit
    {
        public string Name { get; private set; }

        public ChipDiggerBit(string name)
        {
            Name = name;
        }

        public ChipDiggerBitState GetState()
        {
            return new ChipDiggerBitState
            {
                Name = Name
            };
        }

        public static ChipDiggerBit From(ChipDiggerBitState diggerBit)
        {
            return new ChipDiggerBit(diggerBit.Name);
        }
    }
}