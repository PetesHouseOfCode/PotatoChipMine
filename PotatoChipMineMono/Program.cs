using System;
using SadConsole;
using Microsoft.Xna.Framework;
using Console = SadConsole.Console;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PotatoChipMineMono
{
    class Program
    {
        static void Main(string[] args)
        {
            SadConsole.Game.Create(125, 40);

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
