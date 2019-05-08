using PotatoChipMine.Core.GameEngine;
using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Models
{
    public class GameRoom : IGameEntity
    {
        protected string[] Greeting { get; set; }
        protected GameState GameState { get; set; }
        public CommandsGroup CommandsGroup { get; protected set; }
        protected GameMode ActiveMode { get; set; }
        public string Name { get; set; }
        private Scene roomScene;

        protected GameRoom(
            GameState gameState,
            string[] greeting,
            GameMode activeMode)
        {
            ActiveMode = activeMode;
            GameState = gameState;
            Greeting = greeting;
        }

        public virtual void EnterRoom()
        {
            Game.WriteLine(string.Join(Environment.NewLine, Greeting), PcmColor.Cyan);
            GameState.CurrentRoom = this;
        }

        public void HandleInput(UserCommand command)
        {
            CommandsGroup.ExecuteCommand(command, GameState);
        }

        public void Update(Frame frame)
        {
        }
    }
}