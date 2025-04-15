using System;
using System.Collections.Generic;
using UnityEngine;

public class NavObstacle : MonoBehaviour // 자기 좌표 변경되면 알려주기
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
