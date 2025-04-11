using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] allItems;  // 게임 내 아이템 목록

    // 아이템 데이터 얻기
    public Item GetItem(int index)
    {
        if (index >= 0 && index < allItems.Length)
        {
            return allItems[index];
        }
        return null;
    }
}
