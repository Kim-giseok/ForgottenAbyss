using System.Collections.Generic;
using UnityEngine;

public static class DataLoadUtil
{
    public static List<T> LoadJsonData<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"DataLoadUtil Load Fail : {path}");
            return new List<T>();
        }

        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(textAsset.text);
        return wrapper.Items ?? new List<T>();
    }
}

[System.Serializable]
public class Wrapper<T>
{
    public List<T> Items;
}