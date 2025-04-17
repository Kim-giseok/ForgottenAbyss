using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // 인벤토리 패널
    [SerializeField] private InventoryController inventoryController;
    public Button closeButton; // 닫기 버튼
    public Transform slotHolder; // 슬롯 부모
    public InventorySlot[] slots; // 슬롯 배열

    private void Awake()
    {
        slots = slotHolder.GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        // 인벤토리 인스턴스가 있다면 이벤트 등록
        if (Inventory.Instance != null)
            Inventory.Instance.onItemChanged += UpdateUI;

        // 인벤토리가 아직 생성되지 않았다면 기다렸다가 UI 갱신
        StartCoroutine(WaitForInventoryAndUpdate());
    }

    private void OnDisable()
    {
        // 이벤트 제거 (null 체크)
        if (Inventory.Instance != null)
            Inventory.Instance.onItemChanged -= UpdateUI;
    }

    private void Start()
    {
        closeButton.onClick.AddListener(CloseInventory); // 닫기 버튼 이벤트
    }

    // 코루틴으로 인벤토리 생성까지 대기 후 UI 갱신
    private IEnumerator WaitForInventoryAndUpdate()
    {
        yield return new WaitUntil(() => Inventory.Instance != null && Inventory.Instance.items != null);
        Inventory.Instance.onItemChanged += UpdateUI;
        UpdateUI();
    }

    // UI 갱신
    private void UpdateUI()
    {
        if (Inventory.Instance == null)
        {
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.items.Count)
            {
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
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);

        if (isActive)
        {
            inventoryController.InitializeInventory(); // 인벤토리 열릴 때 직접 초기화
        }
    }

    // 인벤토리 닫기
    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
