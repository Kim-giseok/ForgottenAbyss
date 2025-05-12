#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EnemiesLoader
{
    public static bool IsLoaded;

    public static EnemiesViewInfoSO enemiesInfoSO { get; private set; }
    private static readonly Dictionary<string, EnemyStatSO> _enemiesStatSOList = new();
    private static readonly Dictionary<string, EnemySoundSO> _enemiesSoundSOList = new();
    
    public static EnemyStatSO GetStatSO(string name) => _enemiesStatSOList[name];
    public static EnemySoundSO GetSoundSO(string name) => _enemiesSoundSOList[name];
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
    private static void OnLoad()
    {
        Addressables.LoadAssetAsync<EnemiesViewInfoSO>("EnemiesViewInfoSO").Completed += (handle) =>
        {
            enemiesInfoSO = handle.Result;
        };
        
        // 중복 키 관련 문제가 간혹 발생 확인 필요
        Addressables.LoadAssetsAsync<Object>("EnemySO", null).Completed += (handle) =>
        {
            foreach (var so in handle.Result)
            {
                if (so is EnemyStatSO statSO)
                {
                    _enemiesStatSOList.TryAdd(statSO.enemyName, statSO);
                }

                if (so is EnemySoundSO soundSO)
                {
                    _enemiesSoundSOList.TryAdd(soundSO.enemyName, soundSO);
                }
            }
            
            IsLoaded = true;
        };
    }
}