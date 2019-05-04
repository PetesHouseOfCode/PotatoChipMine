using System;

namespace PotatoChipMine.Core.GameEngine
{
    public class Frame
    {
        public TimeSpan TimeSinceStart { get; }
        public TimeSpan Elapsed { get; }

        private Frame(TimeSpan timeSinceStart, TimeSpan elapsed)
        {
            this.TimeSinceStart = timeSinceStart;
            this.Elapsed = elapsed;
        }

        public static Frame NewFrame(TimeSpan timeSinceStart, TimeSpan elapsed)
        {
            return new Frame(timeSinceStart, elapsed);
        }
    }
}