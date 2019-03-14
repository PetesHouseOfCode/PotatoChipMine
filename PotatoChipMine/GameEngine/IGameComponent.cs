using PotatoChipMine;
using PotatoChipMine.Models;

namespace PotatoChipMine.GameEngine
{
    public interface IGameEntity
    {
        void HandleInput(UserCommand command);
        void Update(Frame frame);
    }
}