using System.Collections.Generic;
using UnityEngine;

public class Enemies
{
    public static Dictionary<string, Node> behaviours = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        behaviours.Add("goblin", Goblin.node);
    }
}