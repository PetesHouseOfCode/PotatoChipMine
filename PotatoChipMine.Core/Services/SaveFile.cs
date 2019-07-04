using System;
using System.Linq;

namespace PotatoChipMine.Core.Services
{
    public class SaveFile
    {
        public string Name { get; }
        public DateTime ModifiedDate { get; }

        private SaveFile(string name, DateTime modifiedDate)
        {
            Name = name;
            ModifiedDate = modifiedDate;
        }

        public static SaveFile Create(string name, DateTime modifiedDate)
        {
            return new SaveFile(name, modifiedDate);
        }
    }
}