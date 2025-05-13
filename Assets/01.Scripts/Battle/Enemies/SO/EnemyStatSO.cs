using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatType { Health, Mana, Stamina, Attack, Speed }

[System.Serializable]
public class EnemyStatInfo { public EnemyStatType statType; public float value; }

[CreateAssetMenu(menuName = "SO/Enemy/StatSO")]
public class EnemyStatSO: EnemySO
{
    public List<EnemyStatInfo> stats;
}