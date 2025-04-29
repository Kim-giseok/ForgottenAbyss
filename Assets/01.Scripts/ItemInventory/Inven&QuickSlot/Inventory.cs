using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

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
        if (item == null) return false;

        items.Add(item);
        onItemChanged?.Invoke();
        return true;
    }

    // 아이템 제거
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        QuickSlotController.Instance.NotifyItemRemoved(item);
        onItemChanged?.Invoke();
    }

    // 외부에서 UI갱신하도록
    public void RefreshInventoryUI()
    {
        onItemChanged?.Invoke();
    }
}
