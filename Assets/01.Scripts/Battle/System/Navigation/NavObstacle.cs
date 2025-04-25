using System;
using System.Collections.Generic;
using UnityEngine;

// navSurface처럼 지형으로 인식하지만 obstacle임을 인지하고 특수하게 행동할 수 있도록 처리
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
