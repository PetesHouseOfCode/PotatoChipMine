using CsvHelper;
using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.GameAchievements;
using PotatoChipMine.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PotatoChipMine.Resources
{
    public class GameItemRepository : IRepository<GameItem>
    {
        readonly string tablePath;

        public GameItemRepository(string tablePath)
        {
            this.tablePath = tablePath;
        }

        public IReadOnlyList<GameItem> GetAll()
        {
            using (var reader = new StreamReader(tablePath))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<GameItem>();
                return records.ToList();
            }
        }
    }
}
