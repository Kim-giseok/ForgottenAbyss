using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : SlotBase
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.magenta;

    public override void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item);
        }
    }

    public void UseItem()
    {
        if (currentItem != null)
        {
            currentItem.Use();
        }

    }

    public void SetSelected(bool selected)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = selected ? selectedColor : normalColor;
        }
    }
}
