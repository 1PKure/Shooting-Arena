using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void Save(GameData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public static GameData Load()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("No save file found.");
                return null;
            }

            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
            return null;
        }
    }

    public static void DeleteSave()
    {
        try
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"DeleteSave failed: {e.Message}");
        }
    }
}