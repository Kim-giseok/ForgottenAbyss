using System.Collections.Generic;
using UnityEngine;

// 비용에 따른 아이템 스포너가 필요할 듯
public class EnemyRewardHandler : MonoBehaviour
{
    public int gold;
    // public List<string> memoryItems;

    public void CreateMemoryItem()
    {
        // if (memoryItems is not { Count: > 0 }) return;
        // var currMemoryItem = memoryItems[Random.Range(0, memoryItems.Count)];
        FieldItemPool.Instance.CreateMemoryItem(transform.position + (Vector3.up * 0.5f), gameObject.name.Replace("(Clone)", ""));
    }

    public void CreateCoin()
    {
        if (gold <= 0) return;
        FieldItemPool.Instance.CreateCoin(transform.position + (Vector3.up * 0.5f), gold);
    }   
}