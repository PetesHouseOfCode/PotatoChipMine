using PotatoChipMine.Core;
using SadConsole;
using System;
using System.Linq;

namespace PotatoChipMineMono.Consoles
{
    class OutputConsole : ScrollingConsole
    {
        readonly IPotatoChipGame game;

        public OutputConsole(IPotatoChipGame game)
            : base(125, 34)
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
}
