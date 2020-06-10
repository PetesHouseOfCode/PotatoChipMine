namespace PotatoChipMine.Core.GameAchievements
{
    public interface IAchievementReward
    {
        int Id { get; }
        int GameItemId { get; }
    }
}