using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Achievements/Achievement Database")]
public class AchievementDatabase : ScriptableObject
{
    public List<AchievementDefinition> definitions;

    public AchievementDefinition GetDefinition(string id)
    {
        if (definitions == null) return null;
        for (int i = 0; i < definitions.Count; i++)
        {
            if (definitions[i] != null && definitions[i].id == id)
                return definitions[i];
        }
        return null;
    }
}