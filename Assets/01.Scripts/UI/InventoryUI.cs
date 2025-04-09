using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    [SerializeField] private GameObject inventoryPanel; // 인벤토리 ui
    public Button closeButton; // 인벤 닫기 버튼

    bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    private void Awake()
    {
        inven = Inventory.invenInstance; // Inventory 싱글톤 초기화
        // slotHolder가 유효한지 체크 후 초기화
        if (slotHolder != null)
        {
            slots = slotHolder.GetComponentsInChildren<Slot>(); // 슬롯 배열 초기화
        }
        else
        {
            Debug.LogError("slotHolder is not assigned!"); // slotHolder가 할당되지 않으면
        }
    }

    private void Start()
    {
        inven.onSlotCountChange += SlotChange; // 슬롯 개수 변화시 호출될 메서드
        inventoryPanel.SetActive(activeInventory); // 인벤창 상태 초기화
        closeButton.onClick.AddListener(CloseInventory); // 인벤창 닫기버튼 이벤트
    }

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inven.SlotCount)
                slots[i].GetComponent<Button>().interactable = true; // 슬롯 활성화 버튼
            else
                slots[i].GetComponent<Button>().interactable = false; // 슬롯 활성화 버튼
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ActiveInventory();
    }

    // 인벤 활성화/ 비활성화 함수
    void ActiveInventory()
    {
        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);
    }

    // 인벤 닫기 ㅎ함수
    void CloseInventory()
    {
        activeInventory = false;
        inventoryPanel.SetActive(activeInventory);
    }

    public void AddSlot()
    {
        inven.SlotCount++; // 슬롯 추가
    }
}
