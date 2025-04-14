using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyAnimators
{
    public static Dictionary<string, AnimatorOverrideController> animators = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        string[] guids = AssetDatabase.FindAssets("t:AnimatorOverrideController", new[] { "Assets/03.Animations/Enemies/Animators" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AnimatorOverrideController animator = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(path);
            animators.Add(animator.name, animator);
        }
    }
}