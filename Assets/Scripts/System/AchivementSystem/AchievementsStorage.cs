using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class AchievementsStorage
{
    private const string FileName = "achievements.json";

    [Serializable]
    private class AchievementStateListWrapper
    {
        public List<AchievementState> items = new List<AchievementState>();
    }

    private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

    public static List<AchievementState> LoadOrEmpty()
    {
        try
        {
            if (!File.Exists(FilePath))
                return new List<AchievementState>();

            string json = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<AchievementState>();

            var wrapper = JsonUtility.FromJson<AchievementStateListWrapper>(json);
            return wrapper?.items ?? new List<AchievementState>();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[AchievementsStorage] Load failed: {e.Message}");
            return new List<AchievementState>();
        }
    }

    public static void Save(IReadOnlyCollection<AchievementState> states)
    {
        try
        {
            var wrapper = new AchievementStateListWrapper
            {
                items = new List<AchievementState>(states)
            };

            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(FilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[AchievementsStorage] Save failed: {e.Message}");
        }

        Debug.Log($"[AchievementsStorage] Saved {states.Count} achievements to: {FilePath}");
    }

    public static void Delete()
    {
        try
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[AchievementsStorage] Delete failed: {e.Message}");
        }
    }
}