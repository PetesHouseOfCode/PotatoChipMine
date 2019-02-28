using System;
using System.Collections.Generic;
using PotatoChipMine.Services;

namespace PotatoChipMine.Models
{
    public class GameRoom
    {
        protected string[] Greeting { get; set; }
        protected static GameUI Ui { get; set; }
        protected GameState GameState { get; set; }
        public CommandsGroup CommandsGroup { get; protected set; }
        protected GameMode ActiveMode { get; set; }

        public string Name { get; set; }

        public GameRoom(
            GameUI ui,
            GameState gameState,
            string[] greeting,
            GameMode activeMode)
        {
            ActiveMode = activeMode;
            GameState = gameState;
            Ui = ui;
            Greeting = greeting;
        }

        public virtual void EnterRoom()
        {
            Ui.FastWrite(Greeting);
            GameState.CurrentRoom = this;
        }

        public void ExecuteCommand(UserCommand command)
        {
            CommandsGroup.ExecuteCommand(Ui, command, GameState);
        }
    }
}