using System.Collections.Generic;
using UnityEngine;

public class EnemyRewardHandler : MonoBehaviour
{
    public int gold;
    public List<GameObject> items;

    public GameObject GetRewardItem()
    {
        if (items is { Count: > 0 }) { return items[Random.Range(0, items.Count)]; }
        return null;
    }
}