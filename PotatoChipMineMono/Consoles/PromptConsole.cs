using Microsoft.Xna.Framework;
using PotatoChipMineMono.Components;
using SadConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoChipMineMono.Consoles
{
    class PromptConsole : ScrollingConsole
    {
        public string Prompt { get; set; }

        private ClassicConsoleKeyboardHandler _keyboardHandlerObject;
        private Timer reporter;

        public PromptConsole()
            : base(125, 40)
        {
            IsVisible = false;
            IsFocused = false;

            _keyboardHandlerObject = new ClassicConsoleKeyboardHandler();

            // Assign our custom handler method from our handler object to this consoles keyboard handler.
            // We could have overridden the ProcessKeyboard method, but I wanted to demonstrate how you
            // can use your own handler on any console type.
            Components.Add(_keyboardHandlerObject);

            // Our custom handler has a call back for processing the commands the user types. We could handle
            // this in any method object anywhere, but we've implemented it on this console directly.
            _keyboardHandlerObject.EnterPressedAction = EnterPressedActionHandler;

            // Enable the keyboard and setup the prompt.
            UseKeyboard = true;
            Cursor.IsVisible = false;
            Prompt = "Prompt> ";


            // Startup description
            ClearText();
            Cursor.Position = new Point(0, 0);
            Cursor.Print("Try typing in the following commands: help, ver, cls, look. If you type exit or quit, the program will end.").NewLine().NewLine();
            _keyboardHandlerObject.CursorLastY = 0;
            TimesShiftedUp = 0;

            Cursor.DisableWordBreak = true;
            Cursor.Print(Prompt);
            Cursor.DisableWordBreak = false;

            reporter = new Timer(TimeSpan.FromMilliseconds(800));
            reporter.TimerElapsed += (sender, e) => { Cursor.Print("Event Reported!"); };
        }

        public void ClearText()
        {
            Clear();
            Cursor.Position = new Point(0, 0);
            _keyboardHandlerObject.CursorLastY = 0;
        }

        private void EnterPressedActionHandler(string value)
        {
            if (value.ToLower() == "help")
            {
                Cursor.NewLine().
                              Print("  Advanced Example: Command Prompt - HELP").NewLine().
                              Print("  =======================================").NewLine().NewLine().
                              Print("  help      - Display this help info").NewLine().
                              Print("  ver       - Display version info").NewLine().
                              Print("  cls       - Clear the screen").NewLine().
                              Print("  look      - Example adventure game cmd").NewLine().
                              Print("  exit,quit - Quit the program").NewLine().
                              Print("  ").NewLine();
            }
            else if (value.ToLower() == "ver")
                Cursor.Print("  SadConsole for MonoGame").NewLine();

            else if (value.ToLower() == "cls")
                ClearText();

            else if (value.ToLower() == "look")
                Cursor.Print("  Looking around you discover that you are in a dark and empty room. To your left there is a computer monitor in front of you and Visual Studio is opened, waiting for your next command.").NewLine();

            else if (value.ToLower() == "exit" || value.ToLower() == "quit")
            {
#if WINDOWS_UAP
                //Windows.UI.Xaml.Application.Current.Exit();       Not working?
#else
                Environment.Exit(0);
#endif
            }

            else
                Cursor.Print("  Unknown command").NewLine();
        }
    }
}
