using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy
{
    public string name => GetType().Name;
    public abstract Dictionary<string, Node> skills { get; protected set; }
    public abstract Node Node { get; protected set; }
}