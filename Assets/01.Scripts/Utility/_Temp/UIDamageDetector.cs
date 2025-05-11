using System;
using UnityEngine;

// 보스 전용
public class UIDamageDetector: MonoBehaviour, IDamagable
{
    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    public void GetDamage(float damage)
    {
        controller.GetDamage(damage);
        
        // UI 값 변경
        // Debug.Log(controller.maxHealth);
        // Debug.Log(controller.health);

        
        if (controller.health <= 0)
        {
            BoltsPool.Instance.Clear();
        }
    }
}