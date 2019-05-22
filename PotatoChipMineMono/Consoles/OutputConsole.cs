using PotatoChipMine.Core;
using SadConsole;
using System;
using System.Linq;

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
}
