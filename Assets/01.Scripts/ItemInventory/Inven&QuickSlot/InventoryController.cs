using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    private void Awake()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>(true).ToList();
        Debug.Log($"[InventoryController] 자동 수집된 슬롯 수: {inventorySlots.Count}");
    }

    private void Start()
    {
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        Debug.Log("[InventoryController] InitializeInventory 호출됨");
        foreach (var slot in inventorySlots)
        {
            Debug.Log($"슬롯 초기화: {slot.name}");
            slot.ClearSlot(); // 데이터 & UI 초기화
        }
    }
}
