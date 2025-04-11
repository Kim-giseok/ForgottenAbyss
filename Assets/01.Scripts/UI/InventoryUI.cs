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
    public InventorySlot[] slots; // 슬롯 배열

    private void Awake()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.onItemChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.onItemChanged -= UpdateUI;
    }

    private void Start()
    {
        closeButton.onClick.AddListener(CloseInventory); // 닫기 버튼 이벤트

        // 인벤토리 UI 초기화 후 강제로 UI 갱신
        UpdateUI();
    }

    //private void Start()
    //{
    //    slots = slotHolder.GetComponentsInChildren<InventorySlot>(); // 슬롯 UI 초기화
    //    Inventory.Instance.onItemChanged += UpdateUI; // 아이템이 변경될 때마다 UI 갱신

    //    closeButton.onClick.AddListener(CloseInventory); // 닫기 버튼 이벤트
    //    inventoryPanel.SetActive(false); // 처음엔 비활성화
    //}

    // UI 갱신
    private void UpdateUI()
    {
        Debug.Log("UpdateUI() 호출됨");

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.items.Count)
            {
                Debug.Log($"슬롯 {i}에 아이템 배치: {Inventory.Instance.items[i].itemName}");
                slots[i].SetItem(Inventory.Instance.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
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
