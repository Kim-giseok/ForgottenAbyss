#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class EnemiesLoader
{
    public static bool IsLoaded;
    public static EnemiesViewInfoSO EnemiesInfoSO { get; private set; }
    private static readonly Dictionary<Type, Dictionary<string, EnemySO>> EnemySO = new();

    public static T Get<T>(string enemyName) where T : ScriptableObject
    {
        if (EnemySO.TryGetValue(typeof(T), out var dict) && dict.TryGetValue(enemyName, out var so)) return so as T;
        return null;
    }
    
    private static void RegisterSO<T>(T so) where T : EnemySO
    {
        var type = so.GetType();
        
        if (!EnemySO.TryGetValue(type, out var dict))
        {
            dict = new Dictionary<string, EnemySO>();
            EnemySO[type] = dict;
        }
        
        dict[so.enemyName] = so;
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
    private static void OnLoad()
    {
        Addressables.LoadAssetAsync<EnemiesViewInfoSO>("EnemiesViewInfoSO").Completed += (handle) =>
        {
            EnemiesInfoSO = handle.Result;
        };
        
        // 중복 키 관련 문제가 간혹 발생 확인 필요
        Addressables.LoadAssetsAsync<EnemySO>("EnemySO", null).Completed += (handle) =>
        {
            foreach (EnemySO so in handle.Result)
            {
                RegisterSO(so);
            }
            IsLoaded = true;
        };
    }
}