using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; //싱글톤

    public List<Item> items = new List<Item>(); // 아이템 목록
    public event Action onItemChanged; // 슬롯 개수 변경 시 호출


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 아이템 추가
    public bool AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("AddItem: 추가하려는 item이 null입니다!");
            return false;
        }

        items.Add(item);
        Debug.Log($"[Inventory] 아이템 추가됨: {item.itemName}");
        onItemChanged?.Invoke();
        return true;
        //items.Add(item);
        //onItemChanged?.Invoke();
        //return true;
    }
    // 아이템 제거
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onItemChanged?.Invoke();
    }
}
