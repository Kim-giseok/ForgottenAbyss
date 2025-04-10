using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class NodeManager
{
    public static List<(string name, Type node)> allNodes = new();
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        var nodeType = typeof(Node);
        var derivedTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type != nodeType && nodeType.IsAssignableFrom(type));
        foreach (var type in derivedTypes) { allNodes.Add((type.Name, type)); }
    }
}