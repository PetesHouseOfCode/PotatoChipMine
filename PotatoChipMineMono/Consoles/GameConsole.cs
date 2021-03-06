using Microsoft.Xna.Framework;
using PotatoChipMine.Core;
using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.Entities;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.GameRooms;
using PotatoChipMine.Core.GameRooms.ClaimsOffice;
using PotatoChipMine.Core.GameRooms.ControlRoom.Services;
using PotatoChipMine.Core.GameRooms.Store.Services;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
using PotatoChipMine.Resources;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using EventLog = PotatoChipMine.Core.EventLog;
using MineGame = PotatoChipMine.Core.GameEngine.Game;

namespace PotatoChipMineMono.Consoles
{
    internal class GameConsole : ContainerConsole, IPotatoChipGame
    {
        private readonly CommandsGroup commandsGroup;
        private readonly GameState gameState;

        private readonly GameEventsConsole events;

        //private readonly GamePersistenceService gamePersistenceService = new GamePersistenceService();
        private readonly HudConsole hud;
        private readonly InputConsole input;
        private readonly OutputConsole output;

        private readonly DataGateway gateway;


        public GameConsole()
        {
            gameState = new GameState
            {
                Running = true
            };

            var rewardsRepo = new RewardRepository(@".\Resources\Dat\rewards.csv");
            var achievementsRepo = new AchievementRepository(@".\Resources\Dat\achievements.csv", gameState);
            var gameItemsRepo = new GameItemRepository(@".\Resources\Dat\gameItems.csv");
            var storeInventoryRepo = new StoryInventoryRepository(@".\Resources\Dat\storeInventory.csv");
            gateway = new DataGateway(rewardsRepo, gameItemsRepo, achievementsRepo, storeInventoryRepo);

            commandsGroup = new TopCommandGroupFactory().Build();
            gameState.Lobby = new LobbyRoom(gameState, new[] { "Welcome to the Lobby" }, GameMode.Lobby, commandsGroup);
            gameState.Store = gameState.Store ?? new MinerStoreFactory(gameState, commandsGroup, gateway).Build();
            gameState.ControlRoom = new ControlRoomFactory(gameState, commandsGroup).Build();
            gameState.ClaimsOffice = new ClaimsOfficeRoomFactory(gameState, commandsGroup, gateway).Build();
            gameState.SaveDirectory = @"c:\chipMiner\saves";
            gameState.GameTime.Start();

            IsVisible = true;
            IsFocused = true;

            StartGame();
            hud = new HudConsole(gameState) { Position = new Point(0, 0) };
            input = new InputConsole(this, gameState) { Position = new Point(1, 34) };
            output = new OutputConsole(this, 83) { Position = new Point(1, 1) };
            events = new GameEventsConsole(this, 83) { Position = new Point(85, 1) };
            Children.Add(hud);
            Children.Add(input);
            Children.Add(output);
            Children.Add(events);
            Global.FocusedConsoles.Set(input);
        }

        public Stack<Scene> SceneStack { get; } = new Stack<Scene>();
        public Scene CurrentScene { get; set; }
        public ConsoleBuffer Output { get; set; } = new ConsoleBuffer();
        public ConsoleBuffer Events { get; set; } = new ConsoleBuffer();
        public GameState GameState { get { return gameState; } }
        public DataGateway Gateway { get { return gateway; } }

        public void ClearConsole(GameConsoles targetConsole = GameConsoles.Output)
        {
            switch (targetConsole)
            {
                case GameConsoles.Events:
                    if (events == null) return;
                    events.Clear();
                    events.Cursor.Position = Point.Zero;
                    break;
                case GameConsoles.Input:
                    if (input == null) return;
                    input.ClearText();
                    break;
                default:
                    if (output == null) return;
                    output.Clear();
                    output.Cursor.Position = Point.Zero;
                    break;
            }
        }

        public void StartGame()
        {
            MineGame.SetMainProcess(this);
            MineGame.Achievements = gateway.GameAchievements.GetAll().ToList();
            gameState.Lobby.EnterRoom();
            MineGame.SwitchScene(Scene.Create(new List<IGameEntity>
            {
                new GameLoaderEntity(gameState)
            }));
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return input.ProcessKeyboard(info);
        }

        public override void Update(TimeSpan timeElapsed)
        {
            var frame = Frame.NewFrame(gameState.GameTime.Elapsed, timeElapsed);
            foreach (var entity in CurrentScene.Entities)
                entity.Update(frame);

            DoEvents();
            base.Update(timeElapsed);
        }

        private void DoEvents()
        {
            foreach (var newEvent in gameState.NewEvents)
            {
                MineGame.WriteLine(newEvent.Message + Environment.NewLine, PcmColor.Green, null, GameConsoles.Events);
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