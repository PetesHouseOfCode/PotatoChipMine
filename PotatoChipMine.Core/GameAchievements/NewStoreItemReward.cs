namespace PotatoChipMine.Core.GameAchievements
{
    public class NewStoreItemReward : IAchievementReward
    {
        public int Count { get; private set; }
        public int Price { get; private set; }
        public int Id { get; private set; }
        public int GameItemId { get; private set; }

        public NewStoreItemReward(int id, int count, int price, int gameItemId)
        {
            Id = id;
            Price = price;
            Count = count;
            GameItemId = gameItemId;
        }
    }
}