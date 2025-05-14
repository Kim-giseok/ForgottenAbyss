using System;
using UnityEngine;

public class UIDamageDetector: MonoBehaviour, IDamagable
{
    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    public void GetDamage(float damage)
    {
        controller.GetDamageByType(damage, EnemyStatusHandler.HitType.Normal);

        if (controller.resourceHandler.Get(EnemyStatType.Health).currValue <= 0)
        {
            BoltsPool.Instance.Clear();
        }
    }
}