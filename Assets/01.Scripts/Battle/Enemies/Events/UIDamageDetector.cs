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
        controller.GetDamage(damage);

        var health = controller.resourceHandler.Get(EnemyStatType.Health);
        var currHealthPercent = (int)((health.value / health.maxValue) * 100f);
        UIManager.Instance.BossHealthUI.SetPercentage(currHealthPercent);
        
        // controller.GetDamageByType(damage, EnemyStatusHandler.HitType.Normal);

        // if (controller.resourceHandler.Get(EnemyStatType.Health).currValue <= 0)
        // {
        //     BoltsPool.Instance.Clear();
        // }
    }

    // notice: disable 인지 destroy 인지 체크 필요
    private void OnDisable()
    {
        var bossHealthUI = UIManager.Instance.BossHealthUI;
        
        bossHealthUI.ClearProfile();
        bossHealthUI.gameObject.SetActive(false);
    }
}