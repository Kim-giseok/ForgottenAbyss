using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    private void Awake()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>(true).ToList(); // 인벤토리 슬롯 자동할당
    }

    private void Start()
    {
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        foreach (var slot in inventorySlots)
        {
            slot.ClearSlot(); // 데이터 & UI 초기화
        }
    }
}
