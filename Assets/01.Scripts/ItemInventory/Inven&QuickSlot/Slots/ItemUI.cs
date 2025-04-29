using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; // 아이템 데이터
    private Transform originalParent; // 원래 부모 저장
    private CanvasGroup canvasGroup; // 드래그할 때 UI 투명도 설정
    private RectTransform rectTransform; // 드래그 UI 위치 설정
    private Canvas dragCanvas; // 가장 위에 있는 Canvas

    private GameObject dragCopy; // 드래그 복제본


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        dragCanvas = GameObject.Find("DragCanvas")?.GetComponent<Canvas>(); // 드래그용
    }

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<Image>().sprite = item.itemIcon;
        gameObject.SetActive(true);

    }

    public void SetDraggable(bool canDrag)
    {
        // isDraggable = canDrag; // 모드 나눌 때 사용
    }

    // 아이템 제거
    public void RemoveItem()
    {
        item = null;
        GetComponent<Image>().sprite = null;
        gameObject.SetActive(false);
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 풀에서 가져오기
        dragCopy = DragItemPool.Instance.Get();
        dragCopy.transform.SetParent(dragCanvas.transform, false);

        var copyItemUI = dragCopy.GetComponent<ItemUI>();
        copyItemUI.SetItem(item);

        dragCopy.transform.position = transform.position;
        var copyCanvasGroup = dragCopy.GetComponent<CanvasGroup>();
        copyCanvasGroup.alpha = 0.6f;
        copyCanvasGroup.blocksRaycasts = false;
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (dragCopy != null)
            dragCopy.transform.position = eventData.position;
    }

    // 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragCopy != null)
        {
            DragItemPool.Instance.Return(dragCopy);
        }

        var dropSlot = eventData.pointerEnter?.GetComponentInParent<SlotBase>();
        var fromSlot = originalParent?.GetComponent<SlotBase>();

        if (dropSlot != null && fromSlot != null && item != null)
        {
            if (dropSlot != fromSlot)
            {
                // 원래 슬롯 비우기
                if (fromSlot is InventorySlot)
                    Inventory.Instance.RemoveItem(fromSlot.currentItem);

                fromSlot.ClearSlot();

                // 새 슬롯에 아이템 넣기
                dropSlot.SetItem(item);

                if (dropSlot is InventorySlot)
                    Inventory.Instance.AddItem(item);

                // 퀵슬롯에 넣을 때는 별도로 추적
                if (dropSlot is QuickSlot quickSlot)
                {
                    QuickSlotController.Instance.SetItemToQuickSlot(quickSlot.SlotIndex, item);
                }
            }
        }

        // 드래그 종료 후 항상 인벤토리 UI 갱신
        Inventory.Instance.RefreshInventoryUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            UIManager.Instance.ShowTooltip(item, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}


