using System.Collections.Generic;
using UnityEngine;

// 비용에 따른 아이템 스포너가 필요할 듯
public class EnemyRewardHandler : MonoBehaviour
{
    public GameObject money;
    public List<GameObject> items;
    
    public GameObject GetRewardItem()
    {
        if (items is { Count: > 0 }) { return items[Random.Range(0, items.Count)]; }
        return null;
    }
}