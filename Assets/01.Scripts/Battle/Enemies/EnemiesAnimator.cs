using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class EnemiesAnimator
{
    public static Dictionary<string, RuntimeAnimatorController> animators = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        Addressables.LoadAssetsAsync<RuntimeAnimatorController>("EnemyAnimator", null).Completed += (handle) =>
        {
            foreach (var animator in handle.Result)
            {
                animators.Add(animator.name, animator);
            }
        };
    }
}