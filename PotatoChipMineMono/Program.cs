using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace PotatoChipMineMono
{
    class Program
    {
        static void Main(string[] args)
        {
            SadConsole.Game.Create(175, 40);

            SadConsole.Game.OnInitialize = Init;
            SadConsole.Game.OnUpdate = Update;
            SadConsole.Game.OnDraw = Draw;

            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        static void Init()
        {
            var console = new Container();
            //console.FillWithRandomGarbage();
            //console.Fill(new Rectangle(3, 3, 23, 3), Color.Violet, Color.Black, 0, 0);
            //console.Print(4, 4, "Hello from SadConsole");

            SadConsole.Global.CurrentScreen = console;
        }


        static void Update(GameTime gameTime)
        {

        }

        static void Draw(GameTime gameTime)
        {

        }
    }
}