using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string savePath => Application.persistentDataPath + "/save.dat";

    public static void Save(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogWarning("No save file found.");
            return null;
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);
    }
}
