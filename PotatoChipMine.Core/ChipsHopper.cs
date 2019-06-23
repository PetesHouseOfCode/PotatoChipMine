namespace PotatoChipMine.Core
{
    public class ChipsHopper
    {
        public int Max { get; set; } = 0;
        public int Count { get; private set; } = 0;
        public bool IsFull => Count >= Max;

        public ChipsHopper(int max)
        {
            Max = max;
        }

        private ChipsHopper(int max, int count)
        {
            Max = max;
            Count = count;
        }

        public static ChipsHopper Restore(int max, int count)
        {
            return new ChipsHopper(max, count);
        }
        
        public void AddChips(int amount)
        {
            if (Count + amount > Max)
            {
                Count = Max;
                return;
            }

            Count += amount;
        }

        public void Empty()
        {
            Count = 0;
        }
    }
}