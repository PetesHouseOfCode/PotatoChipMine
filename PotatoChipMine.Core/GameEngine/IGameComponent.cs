using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.GameEngine
{
    public interface IGameEntity
    {
        void HandleInput(UserCommand command);
        void Update(Frame frame);
    }
}