using PotatoChipMine.Core;
using SadConsole;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using PotatoChipMine.Core.Models;
using Console = SadConsole.Console;

namespace PotatoChipMineMono.Consoles
{
    class OutputConsole : ScrollingConsole
    {
        readonly IPotatoChipGame game;

        public OutputConsole(IPotatoChipGame game, int width = 80, int height = 32)
            : base(width, height)
        {
            this.game = game;
            Cursor.UseStringParser = false;
            Cursor.DisableWordBreak = true;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var characters = game.Output.Read(10).ToList();
            if (characters.Any())
            {
                //HideCommandPrompt();
                foreach (var character in characters)
                    Cursor.Print(new ColoredString(character.Char.ToString(), character.ForegroundColor.ToColor(),
                        character.BackgroundColor.ToColor()));
            }
            else
            {
                //ShowCommandPrompt();
            }

            base.Draw(timeElapsed);
        }
    }

    class GameEventsConsole : ScrollingConsole
    {
        readonly IPotatoChipGame game;

        public GameEventsConsole(IPotatoChipGame game, int width = 80, int height = 32)
            : base(width, height)
        {
            this.game = game;
            Cursor.DisableWordBreak = true;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var characters = game.Events.Read(10).ToList();
            if (characters.Any())
            {
                foreach (var character in characters)
                    Cursor.Print(new ColoredString(character.Char.ToString(), character.ForegroundColor.ToColor(),
                        character.BackgroundColor.ToColor()));
            }

            base.Draw(timeElapsed);
        }
    }

    public class HudConsole : Console
    {
        private readonly GameState _gameState;

        public HudConsole(GameState gameState, int width = 170, int height = 1)
            : base(width, height)
        {
            _gameState = gameState;
            Cursor.DisableWordBreak = true;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            if (_gameState != null && _gameState.Miner != null)
            {
                Clear();
                var hudString =
                    $"Miner:{_gameState.Miner.Name}        Tokens:{_gameState.Miner.TaterTokens}          Chips:{_gameState.Miner.InventoryItems.First(x => x.Name == "chips").Count}" +
                    $"                                                     Running Diggers:{_gameState.Miner.Diggers.Count(x => !x.Hopper.IsFull && x.Durability > 0).ToString()}" +
                    $"     Broken Diggers:{_gameState.Miner.Diggers.Count(x=>x.Durability==0)}          Full Diggers:{_gameState.Miner.Diggers.Count(x=>x.Hopper.IsFull)}";
                Cursor.Position = new Point(0,0);
                Cursor.Print(new ColoredString(hudString, Color.LightYellow, Color.DarkSlateBlue));
            }

            base.Draw(timeElapsed);
        }
    }
}
