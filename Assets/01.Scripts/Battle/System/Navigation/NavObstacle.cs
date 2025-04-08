using System.Collections.Generic;
using UnityEngine;

public class NavObstacle : MonoBehaviour
{
    public static List<GameObject> obstacles { get; private set; } = new();
    private void Awake()
    {
        obstacles.Add(gameObject);
    }
}
