using System;
using System.Linq;
using PotatoChipMine.Core;
using SadConsole;

namespace PotatoChipMineMono.Consoles
{
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
}