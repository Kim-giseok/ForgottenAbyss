using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventorySlot : SlotBase
{
    private ItemUI itemUI;

    private void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item);
        }
    }

    public override void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;

        if (itemUI != null)
        {
            itemUI.SetItem(item);
            itemUI.gameObject.SetActive(true); // 아이템 들어왔을 때만 활성화
        }
    }

    public override void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;

        if (itemUI != null)
        {
            itemUI.RemoveItem();
            itemUI.gameObject.SetActive(false); // 비활성화
        }
    }
}

