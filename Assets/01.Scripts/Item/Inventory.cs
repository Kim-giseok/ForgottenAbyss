using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance; // 싱글톤
    public List<Item> items = new List<Item>(); // 아이템 목록
    public int maxSlots = 20; // 최대 슬롯 개수

    // 슬롯 개수 변경 시 호출될 델리게이트
    public delegate void OnItemChanged();
    public event OnItemChanged onItemChanged;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 아이템 추가
    public bool AddItem(Item item)
    {
        if (items.Count < maxSlots)
        {
            items.Add(item);
            onItemChanged?.Invoke();  // 아이템 추가 후 UI 갱신
            return true;
        }
        return false;
    }

    // 아이템 제거
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            onItemChanged?.Invoke();  // 아이템 제거 후 UI 갱신
        }
    }
}
