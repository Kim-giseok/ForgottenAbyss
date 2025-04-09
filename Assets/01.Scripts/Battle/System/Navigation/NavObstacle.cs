using System;
using System.Collections.Generic;
using UnityEngine;

public class NavObstacle : MonoBehaviour
{
    public static List<GameObject> obstacles { get; private set; } = new();

    private void OnEnable()
    {
        obstacles.Add(gameObject);
    }

    private void OnDisable()
    {
        obstacles.Remove(gameObject);
    }
}
