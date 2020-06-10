using CsvHelper;
using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.GameRooms.Store.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PotatoChipMine.Resources
{
    public class StoryInventoryRepository : IRepository<StoreItem>
    {
        readonly string tablePath;

        public StoryInventoryRepository(string tablePath)
        {
            this.tablePath = tablePath;
        }

        public IReadOnlyList<StoreItem> GetAll()
        {
            return GetRecords().Select(x =>
            {
                return new StoreItem
                {
                    BuyingPrice = x.BuyingPrice,
                    SellingPrice = x.SellingPrice,
                    MinCount = x.MinCount,
                    MaxCount = x.MaxCount,
                    GameItemId = x.GameItemId
                };
            }).ToList();
        }

        private IReadOnlyList<StoreItemRecord> GetRecords()
        {
            List<StoreItemRecord> records;
            using (var reader = new StreamReader(tablePath))
            using (var csv = new CsvReader(reader))
            {
                return records = csv.GetRecords<StoreItemRecord>().ToList();
            }
        }

        class StoreItemRecord
        {
            public int Id { get; set; }
            public int StoreId { get; set; }
            public int BuyingPrice { get; set; }
            public int SellingPrice { get; set; }
            public int MinCount { get; set; }
            public int MaxCount { get; set; }
            public int GameItemId { get; set; }
        }
    }
}