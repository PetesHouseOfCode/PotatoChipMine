using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core.Services
{
    public interface ICommandGroupFactory
    {
        CommandsGroup Build();
    }
}