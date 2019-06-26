using System.Collections.Generic;
using System.Linq;
using PotatoChipMine.Core.Models;

namespace PotatoChipMine.Core
{
    public class PersistentGameElement
    {
        public List<Stat> LifetimeStats { get; set; } = new List<Stat>();
        public string Name { get; set; }

        public void UpdateLifetimeStat(string name, long changeBy)
        {
            var stat = LifetimeStats.FirstOrDefault(x => x.Name == name);
            if (stat == null)
            {
                LifetimeStats.Add(new Stat { Name = name, Count = changeBy });
                return;
            }

            stat.Count += changeBy;
        }

        public long GetLifeTimeStat(string name)
        {
            var stat = LifetimeStats.FirstOrDefault(x => x.Name == name);
            if (stat == null)
                return 0;

            return stat.Count;
        }
    }
}