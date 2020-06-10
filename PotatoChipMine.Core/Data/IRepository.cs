using System;
using System.Collections.Generic;

namespace PotatoChipMine.Core.Data
{
    public interface IRepository<T>
    {
        IReadOnlyList<T> GetAll();
    }
}
