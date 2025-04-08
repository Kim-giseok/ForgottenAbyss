using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; // 아이템 데이터
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public event Action<ItemUI> OnItemDropped; // 아이템 드롭이벤트

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;  // 드래그시 아이템의 투명도 조절
        canvasGroup.blocksRaycasts = false;  // 드래그 중 다른 UI와 충돌하지 않도록 설정
        Debug.Log($"Begin Dragging: {item.itemName}");  // 드래그 시작하면 로그
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;  // 드래그 중에 UI의 위치를 계속 업데이트
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;  // 드래그 끝난후 원래 투명도로 되돌리기
        canvasGroup.blocksRaycasts = true;  // 드래그 끝남 다른 UI와의 충돌 가능
        Debug.Log($"End Dragging: {item.itemName}");  // 드래그 종료시 로그
        OnItemDropped?.Invoke(this);  // 드래그 끝났을때 드롭이벤트 발생
    }

    public void RemoveItem()
    {
        item = null;
        Debug.Log("Item Removed from UI");
    }
}


