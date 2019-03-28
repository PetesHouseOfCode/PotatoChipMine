using System.Collections.Generic;
using PotatoChipMine.GameEngine;
using PotatoChipMine.GameRooms;
using PotatoChipMine.GameRooms.ControlRoom;
using PotatoChipMine.GameRooms.Store;

namespace PotatoChipMine.Models
{
    public class GameState
    {
        public Miner Miner { get; set; }
        public GameMode Mode { get; set; }
        public List<GameEvent> NewEvents { get; set; } = new List<GameEvent>();
        public bool Running { get; internal set; }
        public GameRoom CurrentRoom {get; set;}
        public MinerStore Store { get; internal set; }
        public DiggerControlRoom ControlRoom { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; } = string.Empty;
        public List<EventLog> EventsHistory { get; set; } = new List<EventLog>();
        public LobbyRoom Lobby { get; set; }
        public string PromptText { get; set; }
    }
}