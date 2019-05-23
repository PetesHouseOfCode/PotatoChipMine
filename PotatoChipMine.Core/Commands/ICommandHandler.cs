using System;

namespace PotatoChipMine.Core.Commands
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}
