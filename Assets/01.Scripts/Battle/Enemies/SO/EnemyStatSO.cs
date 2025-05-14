using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatType { Health, Mana, Stamina, Attack, Speed, SightRange, AttackRange, BoundaryRange }

[System.Serializable]
public class EnemyStatInfo
{
    public EnemyStatType statType; 
    public float value;

    public EnemyStatInfo(EnemyStatType statType, float value)
    {
        this.statType = statType;
        this.value = value;
    }
}

[CreateAssetMenu(menuName = "SO/Enemy/StatSO")]
public class EnemyStatSO: EnemySO
{
    public List<EnemyStatInfo> stats;
}