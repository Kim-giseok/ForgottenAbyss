using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotBase : MonoBehaviour, IDropHandler
{
    public Item currentItem; // 지금 이 슬롯에 들어있는 아이템
    public Image iconImage; // 슬롯 위에 표시되는 아이템 아이콘

    // 아이템을 넣는 함수
    public virtual void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;
    }

    // 슬롯 비우는 함수
    public virtual void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;

        var itemUI = GetComponentInChildren<ItemUI>(true);
        if (itemUI != null)
        {
            itemUI.RemoveItem();
            itemUI.gameObject.SetActive(false);
        }
    }

    // 드래그한 아이템이 이 슬롯에 드롭될 때 실행되는 함수
    public virtual void OnDrop(PointerEventData eventData)
    {
        var draggedUI = eventData.pointerDrag?.GetComponent<ItemUI>();
        var fromSlot = draggedUI?.transform.parent.GetComponent<SlotBase>();

        if (draggedUI == null || draggedUI.item == null || fromSlot == null)
        {
            Debug.LogWarning("드롭 실패");
            return;
        }

        Item item = draggedUI.item;

        // 인벤 <-> 퀵슬롯 이동
        if (fromSlot is InventorySlot && this is QuickSlot)
            Inventory.Instance.RemoveItem(item);
        else if (fromSlot is QuickSlot && this is InventorySlot)
            Inventory.Instance.AddItem(item);

        // 아이템 세팅
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;

        // UI 이동
        draggedUI.transform.SetParent(transform, false);
        draggedUI.transform.localPosition = Vector3.zero;

        // 원래 슬롯 비움
        fromSlot.ClearSlot();

        eventData.Use(); // 더 이상 OnEndDrag에서 복귀 안 하게 함
    }

}
