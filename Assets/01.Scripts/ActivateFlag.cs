using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum FLAGKEY
{
    IGNOREFLAG,
    INTRO_DIALOGUE,
    BASE_BATTLE_TUTORIAL,
    WEAPON_SWAP_TUTORIAL,
    STAGE_1_CLEAR,
    STAGE_2_CLEAR,
    STAGE_3_CLEAR,
    FIRST_WEAPON_EQUIP
}

public static class ActivateFlag
{
    static Dictionary<FLAGKEY, bool> flags = new();
    static string flagSabePath = "activateFlags.json";

    static ActivateFlag()
    {
        var flagSaveData = flags.ToList();
        flagSaveData = DataSave<List<KeyValuePair<FLAGKEY, bool>>>.LoadOrBase(flagSaveData, flagSabePath);

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
        var flagSaveData = flags.ToList();
        DataSave<List<KeyValuePair<FLAGKEY, bool>>>.SaveData(flagSaveData, flagSabePath);
    }

    static void SaveFlags(PlayModeStateChange mode)
    {
        if (mode == PlayModeStateChange.ExitingPlayMode)
            SaveFlags();
    }
}