using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum FLAGKEY
{
    IGNOREFLAG,
    INTRO_DIALOGUE,
    BASE_BATTLE_TUTORIAL,
    WEAPON_SWAP_TUTORIAL,
    STAGE_1_CLEAR,
    STAGE_2_CLEAR,
    STAGE_3_CLEAR,
    FIRST_WEAPON_EQUIP,
    NPCTRIGGER,
    PUSH_LABER_TUTORIAL
}

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}

[Serializable]
public class DictionaryList<TKey, TValue>
{
    public List<SerializableKeyValuePair<TKey, TValue>> list;

    public DictionaryList<TKey, TValue> FromDic(Dictionary<TKey, TValue> dic)
    {
        list = new();
        foreach (var obj in dic)
            list.Add(new SerializableKeyValuePair<TKey, TValue> { key = obj.Key, value = obj.Value });

        return this;
    }

    public Dictionary<TKey, TValue> ToDic()
    {
        Dictionary<TKey, TValue> dic = new();

        if (list != null)
            foreach (var obj in list)
                dic[obj.key] = obj.value;

        return dic;
    }
}

public static class ActivateFlag
{
    static Dictionary<FLAGKEY, bool> flags = new();
    static string flagSabePath = "activateFlags.json";

    static ActivateFlag()
    {
        var flagSaveData = new DictionaryList<FLAGKEY, bool>().FromDic(flags);
        flagSaveData = DataSave<DictionaryList<FLAGKEY,bool>>.LoadOrBase(flagSaveData, flagSabePath);
        flags = flagSaveData.ToDic();

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= SaveFlags;
        EditorApplication.playModeStateChanged += SaveFlags;
#elif UNITY_WEBGL

#else
        Application.quitting -= SaveFlags;
        Application.quitting += SaveFlags;
#endif
    }

    public static void ActiveFlag(FLAGKEY key) => flags[key] = true;

    public static bool CheckFlag(FLAGKEY key) => flags.ContainsKey(key) && flags[key];

    static void SaveFlags()
    {
        var flagSaveData = new DictionaryList<FLAGKEY,bool>().FromDic(flags);
        DataSave<DictionaryList<FLAGKEY, bool>>.SaveData(flagSaveData, flagSabePath);
    }

#if UNITY_EDITOR
    static void SaveFlags(PlayModeStateChange mode)
    {
        if (mode == PlayModeStateChange.ExitingPlayMode)
            SaveFlags();
    }
#endif
}