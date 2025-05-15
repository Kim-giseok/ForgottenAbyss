using System;
using UnityEngine;

/// <summary>
/// 인벤토리나 퀵슬롯 등에서 사용하는 아이템 슬롯 데이터
/// UI와는 분리된 순수 데이터 모델
/// </summary>
[Serializable]
public class Slot : ISlot
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;

    public Item Item => item;
    public int Quantity => quantity;

    public bool IsEmpty => item == null || quantity <= 0;

    public bool Use()
    {
        if (item == null || quantity <= 0) return false;

        bool used = item.Use();

        if (used)
        {
            quantity--;
            if (quantity <= 0)
            {
                Clear();
            }
        }

        return used;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

    public void Set(Item newItem, int amount)
    {
        item = newItem;
        quantity = amount;
        Debug.Log($"[Slot] Set 호출됨: {newItem?.name ?? "null"}, 수량: {amount}");
    }

    public void Add(int amount)
    {
        if (item != null)
            quantity += amount;
    }

    public void Remove(int amount)
    {
        quantity -= amount;
        if (quantity <= 0) Clear();
    }

    public bool CanStack(Item other)
    {
        return item != null &&
               item == other && // 참조 비교
               quantity < item.maxStack;
    }
}
