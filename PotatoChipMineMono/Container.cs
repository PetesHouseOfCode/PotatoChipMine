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
            var console = new SplashConsole();
            Children.Add(console);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return base.ProcessKeyboard(info);
        }
    }
}
