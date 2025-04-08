using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public Item currentItem;
    public Image slotImage;
    private ItemUI itemUI; // 슬롯 내에 아이템 UI를 담을 변수

    void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>(); // Slot의 자식에서 ItemUI를 찾아 연결
    }

    // 슬롯에 아이템을 추가하거나 제거하는 메소드
    public void AddItem(Item item)
    {
        currentItem = item;
        slotImage.sprite = item?.itemIcon;  // 아이템이 없으면 기본값으로 처리
        if (itemUI != null)
        {
            itemUI.item = item;  // 아이템 UI의 아이템 설정
        }
    }

    public void RemoveItem()
    {
        currentItem = null;
        slotImage.sprite = null;
        if (itemUI != null)
        {
            itemUI.item = null;  // 아이템 UI 초기화
        }
    }

    // 드래그 앤 드롭 기능 처리
    public void OnDrop(PointerEventData eventData)
    {
        var draggedItemUI = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (draggedItemUI != null && draggedItemUI.item != null)
        {
            AddItem(draggedItemUI.item); // 드래그한 아이템을 슬롯에 추가
            draggedItemUI.RemoveItem();  // 아이템 UI에서 아이템 제거
        }
    }
}

