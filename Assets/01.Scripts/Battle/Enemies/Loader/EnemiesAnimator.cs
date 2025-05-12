using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class EnemiesAnimator
{
    public static bool IsLoaded;
    
    public static Dictionary<string, RuntimeAnimatorController> animators = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Addressables.LoadAssetsAsync<RuntimeAnimatorController>("EnemyAnimator", null).Completed += (handle) =>
        {
            foreach (var animator in handle.Result) { animators.TryAdd(animator.name, animator); }
            IsLoaded = true;
        };
    }
}