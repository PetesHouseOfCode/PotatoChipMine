using System;
using System.Collections.Generic;
using PotatoChipMine.Services;

namespace PotatoChipMine.Models
{
    public class GameRoom
    {
        protected readonly string[] Greeting;
        protected static GameUI Ui;
        protected GameState GameState;
        protected CommandsGroup CommandsGroup;
        protected readonly GameMode ActiveMode;


        public GameRoom(GameUI ui,GameState gameState,string[] greeting,GameMode activeMode)
        {
            ActiveMode = activeMode;
            GameState = gameState;
            Ui = ui;
            Greeting = greeting;
            
        }

        public virtual void EnterRoom()
        {
            Ui.FastWrite(Greeting);
            while (GameState.Mode == ActiveMode) AcceptCommand();
        }

        protected virtual void AcceptCommand()
        {
        }
    }
}