using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResourceHandler: MonoBehaviour
{
    private EnemyController _controller;
    private EnemyStatSO _statSO;
    private readonly Dictionary<EnemyStatType, EnemyStat> _stats = new();

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }
    
    public void Define(EnemyStatSO newStatSO)
    {
        Debug.LogWarning(newStatSO.enemyName);
        if (!newStatSO) return;

        _statSO = newStatSO;
        
        foreach (var statInfo in _statSO.stats)
        {
            var newValue = statInfo.value + statInfo.value * (_controller.Level * 0.1f);
            if (_stats.TryGetValue(statInfo.statType, out var existStat))
            {
                existStat.Set(newValue);
            }
            else
            {
                _stats[statInfo.statType] = new EnemyStat(statInfo.statType, newValue);

            }
        }
    }

    public EnemyStat Get(EnemyStatType statType)
    {
        _stats.TryGetValue(statType, out var stat);
        return stat;
    }

    public void Modify(EnemyStatType statType, float amount)
    {
        if (_stats.TryGetValue(statType, out var stat))
        {
            stat.Modify(amount);
        }
    }
}