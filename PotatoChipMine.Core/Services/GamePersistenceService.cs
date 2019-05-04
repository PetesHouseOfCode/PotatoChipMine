using System.IO;
using Newtonsoft.Json;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.Services
{
    public class GamePersistenceService 
    {
        public GameSave BuildFromGameState(GameState gameState)
        {
            return new GameSave
            {
                Miner = gameState.Miner,
                Mode = gameState.Mode,
                SaveDirectory = gameState.SaveDirectory,
                SaveName = gameState.SaveName
            };
        }
        public void SaveGame(GameSave gameSave)
        {
            if (!Directory.Exists(gameSave.SaveDirectory)) Directory.CreateDirectory(gameSave.SaveDirectory);
            var stream = new StreamWriter(Path.Combine(gameSave.SaveDirectory, gameSave.SaveName + ".json"),
                false);
            stream.Write(JsonConvert.SerializeObject(gameSave));
            stream.Flush();
            stream.Close();
        }

        public void LoadGame(GameState gameState,string gameName)
        {
            if (Directory.Exists(gameState.SaveDirectory))
            {
                if (File.Exists(Path.Combine(gameState.SaveDirectory, $"{gameName}.json")))
                {
                    var str = File.ReadAllText(Path.Combine(gameState.SaveDirectory, $"{gameName}.json"));
                    var loadedGame = JsonConvert.DeserializeObject<GameSave>(str);
                    gameState.Miner = loadedGame.Miner;
                    gameState.Mode = loadedGame.Mode;
                    gameState.SaveDirectory = loadedGame.SaveDirectory;
                    gameState.SaveName = loadedGame.SaveName;
                }
            }
        }

        public FileInfo[] SaveFiles(GameState gameState)
        {
            var dirInfo = new DirectoryInfo(gameState.SaveDirectory);
            FileInfo[] files = dirInfo.GetFiles();
            return files;
        }
    }
}