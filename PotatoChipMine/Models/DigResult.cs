namespace PotatoChipMine.Models
{
    public class DigResult
    {
        public int ChipsDug { get; }
        
        public int DurabilityLost { get; }

        public DigResult(int chipsDug, int durabilityLost)
        {
            ChipsDug = chipsDug;
            DurabilityLost = durabilityLost;
        }
        
        public static readonly DigResult EmptyDig = new DigResult(0,0); 
    }
}