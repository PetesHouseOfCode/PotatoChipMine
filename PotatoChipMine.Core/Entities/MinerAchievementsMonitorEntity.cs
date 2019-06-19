using System.Linq;
using PotatoChipMine.Core.GameEngine;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;

namespace PotatoChipMine.Core.Entities
{
    public class MinerAchievementsMonitorEntity :GameEntity, IGameEntity
    {

        public MinerAchievementsMonitorEntity(GameState gameState) : base(gameState)
        {
        }

        public void HandleInput(UserCommand command)
        {
            //throw new NotImplementedException();
        }

        public override void Update(Frame frame)
        {
            foreach (var minerPotentialAchievement in GameState.Miner.PotentialAchievements)
            {
                var gameAchievement = Game.Achievements.FirstOrDefault(x => x.Name == minerPotentialAchievement.Name);
                if (gameAchievement==null|| !gameAchievement.AchievementReached()) continue;
                gameAchievement.RegisterAchievement();
                GameState.Miner.AttainedAchievements.Add(minerPotentialAchievement);
                Game.WriteLine($"--Achievement: {minerPotentialAchievement.Description} has been attained.",PcmColor.Black,PcmColor.Magenta,GameConsoles.Events);
            }

            GameState.Miner.PotentialAchievements.RemoveAll(x => GameState.Miner.AttainedAchievements.Contains(x));
        }
    }
}