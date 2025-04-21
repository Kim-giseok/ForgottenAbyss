using System;
using UnityEngine;

public class EnemyResourceHandler: MonoBehaviour
{
    private EnemyController controller;
    
    public float health;
    public float attack;
    public float defence;
    public float speed;
    public float mana;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
}