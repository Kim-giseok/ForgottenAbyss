using System;
using UnityEngine;

public class MudHandEvent: MonoBehaviour, IDamagable
{
    private EnemyController controller;
    public GameObject mudeye;

    private IDamagable mudEyeDamagable;
    public int percent;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        mudEyeDamagable = mudeye.GetComponent<IDamagable>();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void GetDamage(float damage)
    {
        mudEyeDamagable.GetDamage(damage * percent / 100);
        controller.GetDamage(damage);
    }
}