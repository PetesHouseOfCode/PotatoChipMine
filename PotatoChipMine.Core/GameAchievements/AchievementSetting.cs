using System;
using System.Collections.Generic;
using System.Linq;

namespace PotatoChipMine.Core.GameAchievements
{
    public class AchievementSetting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> RewardIds { get; set; } = new List<int>();
    }
}