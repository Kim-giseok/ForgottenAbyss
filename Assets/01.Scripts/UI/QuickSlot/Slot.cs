using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public Item currentItem; // 슬롯에 들어있는 아이템
    public Image iconImage; // 아이콘 이미지
    public ItemUI itemUI; // 아이템 UI

    // 아이템 설정
    public void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;

        if (itemUI != null)
            itemUI.SetItem(item); // UI 연결
    }

    // 슬롯 비우기
    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;

        if (itemUI != null)
            itemUI.RemoveItem();
    }

    // 드래그 앤 드롭 처리
    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item); // 아이템 배치
        }
    }

    public void UseItem()
    {
        if (currentItem != null)
        {
            currentItem.Use(); // 아아템 사용
        }
    }
}

