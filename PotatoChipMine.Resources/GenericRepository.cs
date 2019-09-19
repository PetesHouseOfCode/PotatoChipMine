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

    public class AchievementRepository : IRepository<GameAchievement>
    {
        readonly string tablePath;
        readonly GameState gameState;

        public AchievementRepository(string tablePath, GameState gameState)
        {
            this.gameState = gameState;
            this.tablePath = tablePath;
        }

        public IReadOnlyList<GameAchievement> GetAll()
        {
            using (var reader = new StreamReader(tablePath))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<AchievementRecord>();
                var achievements = records.Select(x =>
                {
                    switch (x.AchievementType)
                    {
                        case "InventoryAchievement":
                            return BuildInventoryAchievement(x);
                        case "LifetimeStatAchievement":
                            return BuildLifetimeStatAchievement(x);
                    }

                    return null;
                });


                return achievements.ToList();
            }
        }

        GameAchievement BuildInventoryAchievement(AchievementRecord x)
        {
            return new InventoryAchievement(
                new InventoryAchievementSetting
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        InventoryItemName = x.InventoryItemName,
                        MinAmount = x.Threshold
                    },
                    gameState
                );
        }

        GameAchievement BuildLifetimeStatAchievement(AchievementRecord x)
        {
            return new LifetimeStatAchievement(
                new LifetimeStatAchievementSetting
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        LifetimeStatName = x.LifetimeStatName,
                        MinCount= x.Threshold
                    },
                    gameState
                );
        }

        class AchievementRecord
        {
            public int Id { get; set; }
            public string AchievementType { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string InventoryItemName { get; set; }
            public int Threshold { get; set; }
            public string LifetimeStatName { get; set; }
        }
    }
}
