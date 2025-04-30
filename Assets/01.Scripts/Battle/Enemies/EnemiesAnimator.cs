using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemiesAnimator
{
    public static Dictionary<string, RuntimeAnimatorController> animators = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        RuntimeAnimatorController[] loadedAnimators = 
            Resources.LoadAll<RuntimeAnimatorController>("EnemyAnimators");

        foreach (var animator in loadedAnimators)
        {
            animators[animator.name] = animator;
        }
    }
}