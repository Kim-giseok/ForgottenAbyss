using System;
using UnityEngine;

public class EnemyResourceHandler: MonoBehaviour
{
    private EnemyStatSO _statSO;
    private EnemyController controller;
    
    private float health;
    private float attack;
    private float defence;
    private float speed;
    private float mana;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    
    // public void Define()
    public void Define(EnemyStatSO newStatSO)
    {
        if (newStatSO) return;
        _statSO = newStatSO;
    }
}