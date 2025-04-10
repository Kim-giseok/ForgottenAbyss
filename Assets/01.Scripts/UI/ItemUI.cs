using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; // 아이템 데이터
    private CanvasGroup canvasGroup; // 드래그할 때 UI 투명도 설정
    private RectTransform rectTransform; // 드래그 UI 위치 설정

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<Image>().sprite = item.itemIcon;  // 아이템 아이콘 표시
    }

    // 아이템 제거
    public void RemoveItem()
    {
        item = null;
        GetComponent<Image>().sprite = null;  // 아이템 아이콘 제거
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;   // 드래그 시 투명도
        canvasGroup.blocksRaycasts = false;  // 다른 UI와 충돌하지 않게 설정
    }

    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position; // 드래그 위치 업데이트
    }

    // 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;   // 드래그 끝나고 원래 상태로
        canvasGroup.blocksRaycasts = true;
    }
}


