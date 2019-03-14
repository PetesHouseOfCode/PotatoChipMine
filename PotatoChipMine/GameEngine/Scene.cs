using System.Collections.Generic;

namespace PotatoChipMine.GameEngine
{
    public class Scene
    {
        public IList<IGameEntity> Entities { get; }

        private Scene(IList<IGameEntity> entities)
        {
            Entities = entities;
        }

        public static Scene Create(IList<IGameEntity> entities)
        {
            return new Scene(entities);
        }
    }
}