using PotatoChipMine.Core;
using SadConsole;
using System;
using System.Linq;

namespace PotatoChipMineMono.Consoles
{
    class OutputConsole : ScrollingConsole
    {
        readonly IPotatoChipGame game;

        public OutputConsole(IPotatoChipGame game,int width = 60, int height = 32)
            : base(width, height)
        {
            this.game = game;
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var characters = game.Output.Read(3).ToList();
            if (characters.Any())
            {
                //HideCommandPrompt();
                foreach (var character in characters)
                    Cursor.Print(new ColoredString(character.Char.ToString(), character.ForegroundColor.ToColor(), character.BackgroundColor.ToColor()));
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

        public GameEventsConsole(IPotatoChipGame game, int width = 60, int height = 32)
            : base(width, height)
        {
            this.game = game;
            
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            var characters = game.Events.Read(3).ToList();
            if (characters.Any())
            {
                foreach (var character in characters)
                    Cursor.Print(new ColoredString(character.Char.ToString(), character.ForegroundColor.ToColor(), character.BackgroundColor.ToColor()));
            }
            base.Draw(timeElapsed);
        }
    }
}
