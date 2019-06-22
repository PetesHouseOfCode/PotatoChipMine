using PotatoChipMine.Core.GameRooms;
using PotatoChipMine.Core.GameRooms.ControlRoom;
using PotatoChipMine.Core.GameRooms.Store;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Models
{
    public class GameState
    {
        public Miner Miner { get; set; }
        public GameMode Mode { get; set; }
        public List<GameEvent> NewEvents { get; set; } = new List<GameEvent>();
        public bool Running { get; set; }
        public GameRoom CurrentRoom {get; set;}
        public MinerStore Store { get; set; }
        public DiggerControlRoom ControlRoom { get; set; }
        public string SaveDirectory { get; set; }
        public string SaveName { get; set; } = string.Empty;
        public List<EventLog> EventsHistory { get; set; } = new List<EventLog>();
        public LobbyRoom Lobby { get; set; }
        string promptText;
        public string PromptText
        {
            get
            {
                return promptText;
            }
            set
            {
                if (promptText == value)
                    return;

                promptText = value;
                PromptTextChanged?.Invoke();
            }
        }

        public Action PromptTextChanged { get; set; }
    }
}