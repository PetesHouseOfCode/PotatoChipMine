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

        public override void HandleInput(UserCommand command)
        {
            //throw new NotImplementedException();
        }

        public override void Update(Frame frame)
        {
            foreach (var achievement in
                Game.Achievements
                .Where(x => GameState.Miner.PotentialAchievements.Any(y => y.Name == x.Name)))
            {
                achievement.CheckAchievement();
            }
        }
    }
}