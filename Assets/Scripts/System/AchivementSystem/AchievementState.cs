using System;

[Serializable]
public class AchievementState
{
    public string id;
    public int currentValue;
    public bool unlocked;
    public long unlockedUnixTime; // optional

    public AchievementState(string id)
    {
        this.id = id;
        currentValue = 0;
        unlocked = false;
        unlockedUnixTime = 0;
    }
}