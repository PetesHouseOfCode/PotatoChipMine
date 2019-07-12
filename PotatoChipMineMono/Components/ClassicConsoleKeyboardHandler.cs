using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PotatoChipMineMono.Consoles;
using SadConsole;
using SadConsole.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotatoChipMineMono.Components
{
    public class ClassicConsoleKeyboardHandler : KeyboardConsoleComponent
    {
        // This holds the row that the virtual cursor is starting from when someone is typing.
        public int CursorLastY;
        private string currentInput = "";

        // this is a callback for the owner of this keyboard handler. It is called when the user presses ENTER.
        public Action<string> EnterPressedAction = (s) => { int i = s.Length; };

        public override void ProcessKeyboard(SadConsole.Console consoleObject, SadConsole.Input.Keyboard info, out bool handled)
        {
            // Upcast this because we know we're only using it with a Console type.
            var console = (InputConsole)consoleObject;

            // Check each key pressed.
            foreach (var key in info.KeysPressed)
            {
                // If the character associated with the key pressed is a printable character, print it
                if (key.Character != '\0')
                {
                    currentInput = currentInput + key.Character.ToString();
                }

                // Special character - BACKSPACE
                else if (key.Key == Keys.Back)
                {
                    if (currentInput.Length <= 0)
                        continue;

                    currentInput = currentInput.Substring(0, currentInput.Length - 1);

                }

                // Special character - ENTER
                else if (key.Key == Keys.Enter)
                {
                    EnterPressedAction(currentInput);
                    currentInput = "";
                    console.Cursor.NewLine();
                }
            }

            console.Cursor.DisableWordBreak = true;
            console.Cursor.CarriageReturn();
            console.Cursor.Print("                                                                ");
            console.Cursor.CarriageReturn();
            console.Cursor.Print(console.Prompt + " " + currentInput);
            console.Cursor.DisableWordBreak = false;

            handled = true;
        }
    }
}