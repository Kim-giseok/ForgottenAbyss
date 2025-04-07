using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSave<T> where T : class
{
    public static void SaveData(T data, string path = "savedata.json")
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);

        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);
        File.WriteAllText(filePath, jsonData);

        Debug.Log("file saved in " + filePath);
    }

    public static T LoadData(string path = "savedata.json")
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);

        if (!File.Exists(filePath))
        {
            Debug.Log("file doesn't exist. return null");
            return null;
        }

        string jsonData = File.ReadAllText(filePath);
        T data = JsonUtility.FromJson<T>(jsonData);
        Debug.Log(path + " loaded succesfully");
        return data;
    }

    public static T LoadOrBase(T obj, string path = "savedata.json")
    {
        T data = LoadData(path);

        return data == null ? obj : data;
    }
}
