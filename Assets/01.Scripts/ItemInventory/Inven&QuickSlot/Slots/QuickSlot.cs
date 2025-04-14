using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : SlotBase, IPointerClickHandler
{
    [SerializeField] private GameObject outlineObject; // 선택된 슬롯 테두리

    private int slotIndex = -1;

    public void SetIndex(int index)
    {
        slotIndex = index;
    }

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
            Debug.Log("아이템 사용");
            currentItem.Use();
        }

    }

    public void SetSelected(bool selected)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(selected);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotIndex == -1) return;

        if (QuickSlotController.Instance.SelectedIndex == slotIndex)
        {
            UseItem(); // 이미 선택된 슬롯이면 사용
        }
        else
        {
            QuickSlotController.Instance.SelectSlotFromOutside(slotIndex);
        }
    }
}
