using Microsoft.Xna.Framework;
using PotatoChipMine.Core;
using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms;
using PotatoChipMine.Core.GameRooms.ControlRoom.Services;
using PotatoChipMine.Core.GameRooms.Store.Services;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EventLog = PotatoChipMine.Core.EventLog;
using MineGame = PotatoChipMine.Core.GameEngine.Game;

namespace PotatoChipMineMono.Consoles
{
    class GameConsole : ContainerConsole, IPotatoChipGame
    {
        private readonly CommandsGroup commandsGroup;
        private readonly GameState gameState;
        private readonly GamePersistenceService gamePersistenceService = new GamePersistenceService();
        private Stopwatch gameTime = Stopwatch.StartNew();
        InputConsole input;

        public Stack<Scene> SceneStack { get; } = new Stack<Scene>();
        public Scene CurrentScene { get; set; }
        public ConsoleBuffer Output { get; set; } = new ConsoleBuffer();
        public ConsoleBuffer Events { get; set; } = new ConsoleBuffer();


        public GameConsole()
        {
            gameState = new GameState
            {
                Running = true
            };
            commandsGroup = new TopCommandGroupFactory().Build();
            gameState.Lobby = new LobbyRoom(gameState, new[] { "Welcome to the Lobby" }, GameMode.Lobby, commandsGroup);
            gameState.Store = new MinerStoreFactory(gameState, commandsGroup).BuildMineStore();
            gameState.ControlRoom = new ControlRoomFactory(gameState, commandsGroup).BuildControlRoom();
            gameState.SaveDirectory = @"c:\chipMiner\saves";

            IsVisible = true;
            IsFocused = true;

            StartGame();
            input = new InputConsole(this, gameState) {Position = new Point(1, 34)};
            var output = new OutputConsole(this,63){Position = new Point(1,1)};
            var events = new GameEventsConsole(this, 60) {Position = new Point(65, 1)};
            Children.Add(input);
            Children.Add(output);
            Children.Add(events);
            Global.FocusedConsoles.Set(input);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return input.ProcessKeyboard(info);
        }

        public void StartGame()
        {
            MineGame.SetMainProcess(this);

            gameState.Lobby.EnterRoom();
            MineGame.SwitchScene(Scene.Create(new List<IGameEntity>{
                new GameLoaderEntity(gameState)
            }));
        }

        public override void Update(TimeSpan timeElapsed)
        {
            var frame = Frame.NewFrame(gameTime.Elapsed, timeElapsed);
            foreach (var entity in CurrentScene.Entities)
                entity.Update(frame);

            DoEvents();
            base.Update(timeElapsed);
        }

        private void DoEvents()
        {
            foreach (var newEvent in gameState.NewEvents)
            {
                MineGame.WriteLine(newEvent.Message + Environment.NewLine, PcmColor.Green,null,GameConsoles.Events);
                gameState.EventsHistory.Add(new EventLog
                {
                    Name = newEvent.Name,
                    Description = newEvent.Description,
                    Processed = DateTime.Now.ToString()
                });
            }

            gameState.NewEvents.Clear();
        }

        public override void Draw(TimeSpan timeElapsed)
        {

            base.Draw(timeElapsed);
        }
    }
}
