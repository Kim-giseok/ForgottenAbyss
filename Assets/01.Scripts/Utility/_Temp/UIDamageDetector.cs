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
        controller.GetDamage(damage);

        if (controller.health <= 0)
        {
            BoltsPool.Instance.Clear();
        }
        
        Debug.Log(controller.maxHealth);
        Debug.Log(controller.health);
        // UI 값 변경
    }
}