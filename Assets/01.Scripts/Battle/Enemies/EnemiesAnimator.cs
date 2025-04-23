using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemiesAnimator
{
    public static Dictionary<string, RuntimeAnimatorController> animators = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        string[] guids = AssetDatabase.FindAssets("t:AnimatorController", new[] { "Assets/03.Animations/Enemies/Animators" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            RuntimeAnimatorController animator = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(path);
            
            animators.Add(animator.name, animator);
        }
    }
}