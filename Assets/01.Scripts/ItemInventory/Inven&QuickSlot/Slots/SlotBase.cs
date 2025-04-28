using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SlotBase : MonoBehaviour, IDropHandler
{
    public Item currentItem; // 지금 이 슬롯에 들어있는 아이템
    public Image iconImage; // 슬롯 위에 표시되는 아이템 아이콘

    public abstract void OnDrop(PointerEventData eventData);
    public abstract void SetItem(Item item);

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

}
