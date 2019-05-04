using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.DrawCalls;
using SadConsole.Input;
using SadConsole.Instructions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = SadConsole.Console;

namespace PotatoChipMineMono.Consoles
{
    public class SplashConsole : ScrollingConsole
    {
        private double _gradientPositionX = -10;
        Console splashConsole;
        private readonly Point _consoleImagePosition = new Point(0, 0);

        public SplashConsole()
            :base(125, 40)
        {
            Init();
        }

        /// <summary>
        /// Creates a new scrolling console with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the surface.</param>
        /// <param name="height">The height of the surface.</param>
        public SplashConsole(int width, int height) : base(width, height)
        {
            Init();
        }

        private void Init()
        {
            splashConsole = new Console(140, 40);
            ShowIntro(splashConsole);

            splashConsole.Tint = Color.Black;

            const string textTemplate = "Potato Chip Miner ";
            var text = new System.Text.StringBuilder(Width * Height);

            for (var i = 0; i < Width * Height; i++)
            {
                text.Append(textTemplate);
            }
            Print(0, 0, text.ToString(), Color.Black, Color.Transparent);

            var animation = new InstructionSet()
                .Wait(TimeSpan.FromSeconds(0.2d))
                .Code(MoveGradient)
                .Code((console, delta) =>
                {
                    console.Fill(Color.Black, Color.Transparent, 0);
                    return true;
                })
                .Wait(TimeSpan.FromSeconds(2.5))
                .Instruct(new FadeTextSurfaceTint(
                    splashConsole,
                    new ColorGradient(Color.Black, Color.Transparent),
                    TimeSpan.FromSeconds(2)))
                .Wait(TimeSpan.FromSeconds(3))
                .InstructConcurrent(new FadeTextSurfaceTint(splashConsole,
                                                      new ColorGradient(Color.Transparent, Color.Black),
                                                      TimeSpan.FromSeconds(2)),

                                        new FadeTextSurfaceTint(this,
                                                      new ColorGradient(Color.Transparent, Color.Black),
                                                      TimeSpan.FromSeconds(1.0d)))

                    // Animation has completed, call the callback this console uses to indicate it's complete
                    .Code((con, delta) => { return true; })
                ;

            animation.Finished += (s, e) => Components.Remove(animation);

            Components.Add(animation);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return base.ProcessKeyboard(info);
        }

        public override void Draw(TimeSpan timeElapsed)
        {
            Renderer.Render(splashConsole);
            Global.DrawCalls.Add(new DrawCallScreenObject(splashConsole, _consoleImagePosition, false));

            base.Draw(timeElapsed);
        }

        private bool isDown = true;
        private int gradients = 0;
        bool MoveGradient(Console console, TimeSpan delta)
        {
            if(isDown)
                _gradientPositionX += delta.TotalSeconds * 25;
            else
                _gradientPositionX -= delta.TotalSeconds * 25;

            if (_gradientPositionX > Height + 10)
            {
                isDown = false;
                gradients++;
                if (gradients > 1)
                    return true;
            }

            if (_gradientPositionX < -10)
                isDown = true;

            var colors = new[] { Color.Black, Color.Yellow, Color.White, Color.Green, Color.Black };
            var colorStops = new[] { 0f, 0.2f, 0.5f, 0.8f, 1f };

            Algorithms.GradientFill(Font.Size, new Point(0, Convert.ToInt32(_gradientPositionX)), 10, 0, new Rectangle(0, 0, Width, Height), new ColorGradient(colors, colorStops), SetForeground);

            return false;
        }

        void ShowIntro(Console console)
        {
            var y = 1;

            foreach (var line in intro)
            {
                var x = 1;
                foreach (var c in line)
                {
                    int output = 63;

                    Debug.Write($"{(int)c}- {c}");
                    switch ((int)c)
                    {
                        case 9556:
                            output = 201;
                            break;
                        case 9552:
                            output = 205;
                            break;
                        case 9559:
                            output = 187;
                            break;
                        case 9553:
                            output = 186;
                            break;
                        case 9562:
                            output = 200;
                            break;
                        case 9565:
                            output = 188;
                            break;
                        default:
                            output = Convert.ToInt32(Encoding.Convert(Encoding.UTF8, Encoding.ASCII, Encoding.UTF8.GetBytes(new char[] { c }))[0]);
                            break;
                    }

                    console.SetGlyph(x, y, output);
                    x++;
                }

                Debug.WriteLine("");

                y++;
            }
        }

        private static readonly List<string> intro = new List<string>()
        {
            "╔════════════════════════════════════════════════════════════════════════════════════════╗",
            "║       _____________                                                                    ║",
            @"║      //            \\                   ||                        ||                   ║",
            @"║      ||             ||                  ||                        ||                   ║",
            @"║      ||             ||               ////////                  ////////                ║",
            @"║      ||             //    ______        ||        _______         ||         ______    ║",
            @"║      ||____________//    /      \       ||       /       \        ||        /      \   ║",
            @"║      ||                 |        |      ||      |         |       ||       |        |  ║",
            @"║      ||                 |        |      ||      |         |\      ||       |        |  ║",
            @"║      ||                  \______/       ||       \_______/\ \     ||        \______/   ║",
            @"║                                                                                        ║",
            @"║        _____________                                                                   ║",
            @"║      //             \\       ||                                                        ║",
            @"║      ||              ||      ||                                                        ║",
            @"║      ||                      ||                \\                                      ║",
            @"║      ||                      ||_________                  _______                      ║",
            @"║      ||                      ||         \\      ||      //       \\                    ║",
            @"║      ||                      ||         ||      ||     ||         ||                   ║",
            @"║      ||              ||      ||         ||      ||     ||         ||                   ║",
            @"║      \\_____________//       ||         ||      ||     ||_________//                   ║",
            @"║                                                        ||                              ║",
            @"║                                                        ||                              ║",
            @"║      ||\\            //||                              ||                              ║",
            @"║      || \\          // ||     \\                                                       ║",
            @"║      ||  \\        //  ||           \|_________       _______       \\_______          ║"  ,
            @"║      ||   \\      //   ||     ||    ||        \\    //       \\     ||      \\         ║",
            @"║      ||    \\    //    ||     ||    ||         ||   ||_______//     ||                 ║",
            @"║      ||     \\  //     ||     ||    ||         ||   ||              ||                 ║",
            @"║      ||      \\//      ||     ||    ||         ||   \\_______//     ||                 ║",
            @"║                                                                                        ║",
            @"╚════════════════════════════════════════════════════════════════════════════════════════╝"
        };
    }
}
