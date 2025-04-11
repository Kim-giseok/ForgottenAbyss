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
    }

    // 드래그한 아이템이 이 슬롯에 드롭될 때 실행되는 함수
    public abstract void OnDrop(PointerEventData eventData);
}
