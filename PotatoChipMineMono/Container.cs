using PotatoChipMineMono.Consoles;
using SadConsole;
using SadConsole.Input;
using System;
using System.Linq;

namespace PotatoChipMineMono
{
    public class Container : ContainerConsole
    {
        public Container()
        {
            var console = new SplashConsole() { SplashDone = SplashCompleted };
            Children.Add(console);
            SplashCompleted();
        }

        public void SplashCompleted()
        {
            var console = new GameConsole();
            Children.Clear();
            Children.Add(console);
            console.IsVisible = true;
            console.IsFocused = true;

            Global.FocusedConsoles.Set(console);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return base.ProcessKeyboard(info);
        }
    }
}
