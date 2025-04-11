using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : SlotBase
{
    public override void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item);
            dragged.RemoveItem(); // 원래 슬롯에서 제거
        }
    }
}
