using System;
using UnityEngine;

public class EnemyResourceHandler: MonoBehaviour
{
    private EnemyStatSO _statSO;
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
    
    // public void Define()
    public void Define(EnemyStatSO newStatSO)
    {
        if (newStatSO) return;
        _statSO = newStatSO;
    }
}