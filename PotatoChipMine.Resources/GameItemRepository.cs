using CsvHelper;
using PotatoChipMine.Core.Data;
using PotatoChipMine.Core.Models;
using PotatoChipMine.Core.Services;
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
            return GetRecords().Select(x =>
                {
                    switch (x.ItemType)
                    {
                        case "GameItem":
                            return BuildGameItem(x);
                        case "ChipsHopperUpgradeItem":
                            return BuildChipsHopperUpgradeItem(x);
                        case "BitUpgradeItem":
                            return BuildBitUpgradeItem(x);
                    }

                    throw new InvalidDataException($"Bad ItemType \"{x.ItemType}\" on Game Item Id {x.Id}");
                }).ToList();
        }

        GameItem BuildGameItem(GameItemRecord record)
        {
            return new GameItem
            {
                Id = record.Id,
                Name = record.Name,
                PluralizedName = record.PluralizedName,
                Description = record.Description
            };
        }

        ChipsHopperUpgradeItem BuildChipsHopperUpgradeItem(GameItemRecord record)
        {
            return new ChipsHopperUpgradeItem()
            {
                Id = record.Id,
                Name = record.Name,
                PluralizedName = record.PluralizedName,
                Description = record.Description,
                RequiredSlotLevel = record.RequiredSlotLevel.Value,
                Level = record.Level.Value,
                Size = record.Size.Value
            };
        }
        BitUpgradeItem BuildBitUpgradeItem(GameItemRecord record)
        {
            return new BitUpgradeItem
            {
                Id = record.Id,
                Name = record.Name,
                PluralizedName = record.PluralizedName,
                Description = record.Description,
                RequiredSlotLevel = record.RequiredSlotLevel.Value,
                Level = record.Level.Value,
                Min = record.Min.Value,
                Max = record.Max.Value
            };
        }

        private IReadOnlyList<GameItemRecord> GetRecords()
        {
            using (var reader = new StreamReader(tablePath))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<GameItemRecord>();
                return records.ToList();
            }
        }

        class GameItemRecord
        {
            public int Id { get; set; }
            public string ItemType { get; set; }
            public string Name { get; set; }
            public string PluralizedName { get; set; }
            public string Description { get; set; }
            public int? RequiredSlotLevel { get; set; }
            public int? Level { get; set; }
            public int? Size { get; set; }
            public int? Min { get; set; }
            public int? Max { get; set; }
        }
    }
}
