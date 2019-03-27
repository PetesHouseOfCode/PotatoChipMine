using System;

namespace PotatoChipMine.GameEngine
{
    public static class Game
    {
        private static MainProcess mainProcess;
        public static void SetMainProcess(MainProcess mainProcess)
        {
            Game.mainProcess = mainProcess;
        }

        public static void SwitchScene(Scene scene)
        {
            mainProcess.CurrentScene = scene;
        }

        public static void PushScene(Scene scene)
        {
            mainProcess.SceneStack.Push(mainProcess.CurrentScene);            
            mainProcess.CurrentScene = scene;
        }

        public static void PopScene()
        {
            mainProcess.CurrentScene = mainProcess.SceneStack.Pop();
        }

        public static void Write(ConsoleChar character)
        {
            mainProcess.Output.Write(character);
        }

        public static void Write(string text, ConsoleColor color)
        {
            foreach (var c in text)
            {
                mainProcess.Output.Write(new ConsoleChar(c, color));
            }
        }
    }    
}