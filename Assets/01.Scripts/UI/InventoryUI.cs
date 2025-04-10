using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // 인벤토리 패널
    public Button closeButton; // 닫기 버튼
    public Transform slotHolder; // 슬롯 부모
    private Slot[] slots; // 슬롯 배열

    private void Start()
    {
        slots = slotHolder.GetComponentsInChildren<Slot>(); // 슬롯 UI 초기화
        Inventory.Instance.onItemChanged += UpdateUI; // 아이템이 변경될 때마다 UI 갱신

        closeButton.onClick.AddListener(CloseInventory); // 닫기 버튼 이벤트
        inventoryPanel.SetActive(false); // 처음엔 비활성화
    }

    // UI 갱신
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.items.Count)
            {
                slots[i].SetItem(Inventory.Instance.items[i]);  // 슬롯에 아이템 설정
            }
            else
            {
                slots[i].ClearSlot();  // 슬롯 비우기
            }
        }
    }

    // 인벤토리 활성화/비활성화
    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    // 인벤토리 닫기
    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
