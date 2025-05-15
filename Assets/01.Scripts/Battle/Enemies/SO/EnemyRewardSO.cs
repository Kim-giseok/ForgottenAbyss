using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class RewardItemInfo { public Item item; [Range(0, 100)] public int percent; }
public enum RewardType { Experience, Gold }


[CreateAssetMenu(menuName = "SO/Enemy/RewardSO", fileName = "RewardSO")]
public class EnemyRewardSO: EnemySO
{
    public int Experience;
    public int Gold;
    
    public MemorySkillItem memorySkillItem;
    public List<RewardItemInfo> rewardItemInfos;
}