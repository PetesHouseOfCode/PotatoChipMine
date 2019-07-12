using System.Collections.Generic;

namespace PotatoChipMine.Core.GameEngine
{
    public class Scene
    {
        public IReadOnlyList<IGameEntity> Entities { get; }

        private Scene(List<IGameEntity> entities)
        {
            Entities = entities;
        }

        public static Scene Create(List<IGameEntity> entities)
        {
            return new Scene(entities);
        }
        public static Scene Create(IGameEntity entities)
        {
            return new Scene(new List<IGameEntity> { entities });
        }
    }
}