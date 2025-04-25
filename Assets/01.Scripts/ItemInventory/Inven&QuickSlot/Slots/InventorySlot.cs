using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotMode { Editable, ReadOnly }
public class InventorySlot : SlotBase, IPointerClickHandler
{
    public SlotMode mode = SlotMode.Editable; // 기본값은 일반모드
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
            itemUI.gameObject.SetActive(true);
            itemUI.SetItem(item);
            itemUI.SetDraggable(mode == SlotMode.Editable);
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
            itemUI.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null)
            return;

        // 아이템이 장착 가능한 타입이면
        if (currentItem.itemType == ItemType.Equip)
        {
            EquipmentManager.Instance.EquipArmor(currentItem as ArmorSO); // 캐스팅 주의
        }
    }
}

