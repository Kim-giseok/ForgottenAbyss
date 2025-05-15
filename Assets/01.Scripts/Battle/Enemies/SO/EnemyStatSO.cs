using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatType
{
    Health = 0,
    Mana = 1,
    Stamina = 2,
    Attack = 3,
    Defense = 7,
    Evasion = 6,
    Speed = 4,
    SightRange = 5,
}

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