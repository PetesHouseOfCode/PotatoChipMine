using System;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class DiggerDurabilityState
    {
        public int Current { get; set; }
        public int Max { get; set; }
        public float Modifier { get; set; }
    }
}
