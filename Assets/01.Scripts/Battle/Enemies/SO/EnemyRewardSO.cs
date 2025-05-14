using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class RewardItemInfo { public GameObject item; public int percent; }
public enum RewardType { Experience, Gold }


[CreateAssetMenu(menuName = "SO/Enemy/RewardSO", fileName = "RewardSO")]
public class EnemyRewardSO: EnemySO
{
    public int Experience;
    public int Gold;
    public List<RewardItemInfo> rewardItemInfos;
}