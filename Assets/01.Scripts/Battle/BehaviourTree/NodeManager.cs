using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

// database 방식으로 변경될 예정
public class NodeManager
{
    public static List<(string name, Type node)> allNodes = new();
    
    // 모든 노드를 알아야하는 현상 발생
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var nodeType = typeof(Node);
        var derivedTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type != nodeType && nodeType.IsAssignableFrom(type));
        foreach (var type in derivedTypes) { allNodes.Add((type.Name, type)); }
    }
}