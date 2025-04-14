using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemies
{
    private static Dictionary<string, Enemy> behaviours = new();
    private static void Register(string key, Enemy enemy) { behaviours[key] = enemy; }
    public static Enemy Get(string name) => behaviours[name];

    // 비용 관련 문제 체크 필요
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        var enemyType = typeof(Enemy);
    
        var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(enemyType) && !type.IsAbstract);
        
        foreach (var type in derivedTypes)
        {
            Enemy instance = (Enemy)Activator.CreateInstance(type);
            Register(instance.name, instance);
        }
    }
}