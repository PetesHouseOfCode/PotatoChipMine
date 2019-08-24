using PotatoChipMine.Core.Services.PersistenceService;
using System;
using System.Linq;

namespace PotatoChipMine.Core.Models.DiggerUpgrades
{
    public class DiggerDurability
    {
        public int Current { get; set; }
        public int Max { get; set; }

        private float modifier = 0.2f;

        private DiggerDurability(DiggerDurabilityState state)
            : this(state.Current, state.Max, state.Modifier)
        {
        }

        private DiggerDurability(int current, int max, float modifier)
        {
            Current = current;
            Max = max;
            this.modifier = modifier;
        }

        public bool NeedsService()
        {
            return Current <= 0;
        }

        public void Service()
        {
            Current = Max;
        }

        public void Damage(int damage)
        {
            Current -= damage;
            Current = Current < 0 ? 0 : Current;
        }

        public DiggerDurabilityState GetState()
        {
            return new DiggerDurabilityState
            {
                Current = Current,
                Max = Max,
                Modifier = modifier
            };
        }

        public static DiggerDurability From(DiggerDurabilityState state)
        {
            return new DiggerDurability(state);
        }
    }
}