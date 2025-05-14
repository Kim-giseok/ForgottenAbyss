using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // 임시 방편용 아이템 데이터 베이스

    public static ItemDatabase Instance;
    private Dictionary<int, Item> itemDict;

    private void Awake()
    {
        Instance = this;
        itemDict = new Dictionary<int, Item>();
    }

    public Item GetItemByID(int id)
    {
        return itemDict.ContainsKey(id) ? itemDict[id] : null;
    }
}
