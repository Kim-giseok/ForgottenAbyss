using System;
using UnityEngine;

public class UIDamageDetector: MonoBehaviour, IDamagable
{
    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    
    // 컨트롤러의 공격이 IDamagable 이 아닌 enemyController로 바로 넘어오는 현상 발생
    public void GetDamage(float damage)
    {
        // controller.GetDamageByType(damage, EnemyStatusHandler.HitType.Normal);

        // if (controller.resourceHandler.Get(EnemyStatType.Health).currValue <= 0)
        // {
        //     BoltsPool.Instance.Clear();
        // }
    }
}