using System;

namespace PotatoChipMine.Core
{
    public class ConsoleSpinner
    {
        private int _currentAnimationFrame;

        public ConsoleSpinner()
        {
            SpinnerAnimationFrames = new[]
            {
                "|",
                "/",
                "-",
                "\\"
            };
        }

        public string[] SpinnerAnimationFrames { get; set; }

        public void UpdateProgress()
        {
            // Store the current position of the cursor
            var originalX = Console.CursorLeft;
            var originalY = Console.CursorTop;

            // Write the next frame (character) in the spinner animation
            Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);

            // Keep looping around all the animation frames
            _currentAnimationFrame++;
            if (_currentAnimationFrame == SpinnerAnimationFrames.Length) _currentAnimationFrame = 0;

            // Restore cursor to original position
            Console.SetCursorPosition(originalX, originalY);
        }
    }
}
