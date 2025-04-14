using System;
using UnityEngine;

public class EnemyResourceHandler: MonoBehaviour
{
    private EnemyController controller;
    
    public float health;
    public float attack;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
}