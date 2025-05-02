using System;
using System.Collections.Generic;
using System.Linq;
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
    public bool AddItem(Item newItem)
    {
        if (newItem == null) return false;

        // 같은 아이템 찾기 (이름 기반 Equals() 적용됨)
        var existing = items.FirstOrDefault(i => i.Equals(newItem) && i.currentAmount < i.maxStack);

        if (existing != null)
        {
            existing.currentAmount++;
        }
        else
        {
            // 아이템 복사해서 넣기
            Item clone = Instantiate(newItem);
            clone.currentAmount = 1;
            items.Add(clone);
        }

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

    public bool RemoveItemByReference(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                if (item.currentAmount > 1)
                {
                    item.currentAmount--;
                }
                else
                {
                    items.RemoveAt(i);
                }

                // 퀵슬롯에도 동기화
                QuickSlotController.Instance.NotifyItemRemoved(item);

                onItemChanged?.Invoke();
                return true;
            }
        }

        Debug.LogWarning($"[Inventory] 제거 실패: 참조가 일치하는 아이템을 찾을 수 없음 ({item.name})");
        return false;
    }

}
