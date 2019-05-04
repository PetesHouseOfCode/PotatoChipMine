using PotatoChipMineMono.Consoles;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoChipMineMono
{
    public class Container : ContainerConsole
    {
        public Container()
        {
            var console = new SplashConsole() { SplashDone = SplashCompleted };
            Children.Add(console);
        }

        public void SplashCompleted()
        {
            var console = new PromptConsole();
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
