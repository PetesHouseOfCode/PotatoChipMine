using Newtonsoft.Json;
using PotatoChipMine.Core.GameRooms.Store.Models;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PotatoChipMine.Core.Services.PersistenceService
{
    public class GamePersistenceService
    {
        public GameSave BuildFromGameState(GameState gameState)
        {
            return new GameSave
            {
                Miner = gameState.Miner.GetState(),
                MinerStore = gameState.Store.StoreState.GetState(),
                Mode = gameState.Mode
            };
        }

        public void SaveGame(GameState gameState)
        {
            var path = gameState.SaveDirectory;
            var saveName = gameState.SaveName;

            var gameSave = BuildFromGameState(gameState);
            foreach (var digger in gameSave.Miner.Diggers)
            {
                digger.LastDig = digger.LastDig - gameState.GameTime.Elapsed;
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var stream = new StreamWriter(Path.Combine(path, saveName + ".json"),
                false);
            stream.Write(JsonConvert.SerializeObject(gameSave));
            stream.Flush();
            stream.Close();
        }

        public void LoadGame(GameState gameState, string gameName)
        {
            var path = gameState.SaveDirectory;
            var fileName = $"{gameName}.json";
            if (Directory.Exists(path) && File.Exists(Path.Combine(path, fileName)))
            {
                var str = File.ReadAllText(Path.Combine(path, $"{gameName}.json"));
                var loadedGame = JsonConvert.DeserializeObject<GameSave>(str);
                gameState.Miner = Miner.FromState(loadedGame.Miner);
                gameState.Store.StoreState = StoreInventory.From(loadedGame.MinerStore);
                gameState.Mode = loadedGame.Mode;
                gameState.SaveDirectory = path;
                gameState.SaveName = gameName;
                gameState.GameTime.Restart();
            }
        }

        public IList<SaveFile> GetSaveFileNames(GameState gameState)
        {
            var dirInfo = new DirectoryInfo(gameState.SaveDirectory);
            var files = dirInfo.GetFiles();
            return files.Select(x => SaveFile.Create(x.Name, x.LastWriteTime)).ToList();
        }
    }
}