using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickSlot : SlotBase
{
    public override void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item);
            dragged.RemoveItem();
        }
    }

    public void UseItem()
    {
        if (currentItem != null)
        {
            Debug.Log($" Äü½½·Ô ¾ÆÀÌÅÛ »ç¿ë: {currentItem.itemName}");
            currentItem.Use();
        }

    }
}
