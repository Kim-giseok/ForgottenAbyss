using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private Slot[] quickSlots;
    [SerializeField] private Transform quickSlotContent; //퀵슬롯 부모 오브젝트
    [SerializeField] private GameObject SelectedSlotImg; //선택된 슬롯 이미지

    private int selectedSlot; // 선택된 슬롯 인덱스
    [SerializeField] private ItemManager itemManager; // 아이템매니저 참조

    void Start()
    {
        quickSlots = quickSlotContent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;

        // 디버그용 아이템 이름만 추가
        Item testItem = new Item("Potion");
        AddItemToSlot(testItem, 0);  // 첫 퀵슬롯에 아이템 추가

        // 각 슬롯에 대한 아이템 UI 드래그 완료 이벤트 연결
        foreach (var slot in quickSlots)
        {
            var itemUI = slot.GetComponentInChildren<ItemUI>(); // 슬롯의 자식에서 아이템 UI 찾기
            if (itemUI != null)
            {
                itemUI.OnItemDropped += HandleItemDropped; // 아이템 드랍 이벤트 연결
            }
        }
    }

    void Update()
    {
        TryInputNumber();
    }

    private void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
    }

    private void ChangeSlot(int slotNum)
    {
        SelectedSlot(slotNum);
    }

    private void SelectedSlot(int slotNum)
    {
        selectedSlot = slotNum;
        // 선택된 슬롯으로 이미지 이동
        SelectedSlotImg.transform.position = quickSlots[selectedSlot].transform.position;
    }

    private void HandleItemDropped(ItemUI itemUI)
    {
        // 아이템이 드롭될 때마다 디버그 로그 출력
        Debug.Log($"Item Dropped: {itemUI.item.itemName}");
        var droppedItem = itemUI.item;
        if (droppedItem != null)
        {
            AddItemToSlot(droppedItem, selectedSlot);
        }
    }

    public void AddItemToSlot(Item item, int slotIndex)
    {
        quickSlots[slotIndex].AddItem(item);
        Debug.Log($"Item Added to Slot {slotIndex}: {item.itemName}");
    }

    public void RemoveItemFromSlot(int slotIndex)
    {
        quickSlots[slotIndex].RemoveItem();
        Debug.Log($"Item Removed from Slot {slotIndex}");
    }
}
