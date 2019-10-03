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
    public class RewardRepository : IRepository<IAchievementReward>
    {
        readonly string tablePath;

        public RewardRepository(string tablePath)
        {
            this.tablePath = tablePath;
        }

        public IReadOnlyList<IAchievementReward> GetAll()
        {
            return GetRecords().Select(x =>
                {
                    switch (x.RewardType)
                    {
                        case "NewStoreItemReward":
                            return BuildNewStoreItemReward(x);
                    }

                    throw new InvalidDataException($"Bad RewardType \"{x.RewardType}\" on Reward Id {x.Id}");
                }).ToList();
        }

        private NewStoreItemReward BuildNewStoreItemReward(RewardRecord record)
        {
            return new NewStoreItemReward(record.Id, record.Count, record.Price, new GameItem());
        }

        private IReadOnlyList<RewardRecord> GetRecords()
        {
            List<RewardRecord> records;
            using (var reader = new StreamReader(tablePath))
            using (var csv = new CsvReader(reader))
            {
                return records = csv.GetRecords<RewardRecord>().ToList();
            }
        }

        class RewardRecord
        {
            public int Id { get; set; }
            public string RewardType { get; set; }
            public int Count { get; set; }
            public int Price { get; set; }
            public int GameItemId { get; set; }
        }
    }
}
