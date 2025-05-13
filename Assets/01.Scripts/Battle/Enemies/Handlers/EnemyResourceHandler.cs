using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResourceHandler: MonoBehaviour
{
    private EnemyStatSO _statSO;
    private readonly Dictionary<EnemyStatType, EnemyStat> _stats = new();
    
    public void Define(EnemyStatSO newStatSO)
    {
        if (!newStatSO) return;

        _statSO = newStatSO;
        
        foreach (EnemyStatInfo statInfo in _statSO.stats)
        {
            if (_stats.TryGetValue(statInfo.statType, out var existStat))
            {
                existStat.Set(statInfo.value);
            }
            else
            {
                _stats[statInfo.statType] = new EnemyStat(statInfo.statType, statInfo.value);

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