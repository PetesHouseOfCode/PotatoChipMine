using PotatoChipMine.Models;

namespace PotatoChipMine.Services
{
    public interface ICommandGroupFactory
    {
        CommandsGroup Build();
    }
}