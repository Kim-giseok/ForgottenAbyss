using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class RewardItemInfo { public GameObject item; public int percent; }

[CreateAssetMenu(menuName = "SO/Enemy/RewardSO", fileName = "RewardSO")]
public class EnemyRewardSO: EnemySO
{
    public int experience;
    public int gold;
    public List<RewardItemInfo> rewardItemInfos;
}