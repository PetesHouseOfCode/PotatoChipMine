namespace PotatoChipMine.Models
{
    public class GameSave
    {
        public Miner Miner { get; set; }
        public GameMode Mode { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; }

    }
}