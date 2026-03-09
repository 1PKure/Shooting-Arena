public static class GameEvents
{
    public delegate void ScoreChanged(int newScore);
    public delegate void TimeChanged(float remainingTime);
    public delegate void KillChanged(int newKillCount);
    public delegate void GameOver(int finalScore);
    public delegate void Victory(int finalScore);
    public delegate void GameSaved();
    public delegate void GameLoaded(GameData data);

    public delegate void AchievementUnlocked(string achievementId);
    public delegate void AchievementProgress(string achievementId, int current, int target);
}