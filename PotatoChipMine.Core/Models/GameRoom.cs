using System;
using PotatoChipMine.Core.GameEngine;

namespace PotatoChipMine.Core.Models
{
    public class GameRoom : IGameEntity
    {
        protected GameRoom(
            GameState gameState,
            string[] greeting,
            GameMode activeMode)
        {
            ActiveMode = activeMode;
            GameState = gameState;
            Greeting = greeting;
        }

        protected string[] Greeting { get; set; }
        protected GameState GameState { get; set; }
        public CommandsGroup CommandsGroup { get; protected set; }
        protected GameMode ActiveMode { get; set; }
        public string Name { get; set; }

        public void HandleInput(UserCommand command)
        {
            CommandsGroup.ExecuteCommand(command, GameState);
        }

        public void Update(Frame frame)
        {
        }

        public virtual void EnterRoom()
        {
            Game.ClearConsole();
            Game.WriteLine(string.Join(Environment.NewLine, Greeting), PcmColor.Cyan);
            GameState.CurrentRoom = this;
        }
    }
}