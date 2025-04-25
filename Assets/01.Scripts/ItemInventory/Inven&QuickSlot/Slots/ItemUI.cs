using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; // 아이템 데이터
    private Transform originalParent; // 원래 부모 저장
    private CanvasGroup canvasGroup; // 드래그할 때 UI 투명도 설정
    private RectTransform rectTransform; // 드래그 UI 위치 설정
    private Canvas dragCanvas; // 가장 위에 있는 Canvas


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        dragCanvas = GameObject.Find("DragCanvas")?.GetComponent<Canvas>(); // 드래그용
    }

    public void SetDraggable(bool canDrag)
    {
        // isDraggable = canDrag; // 모드 나눌 때 사용
    }

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        item = newItem;
        GetComponent<Image>().sprite = item.itemIcon;
        gameObject.SetActive(true);

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
        originalParent = transform.parent;
        transform.SetParent(dragCanvas.transform, true); // 최상위 캔버스로 이동
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
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 실패 시 원래 자리로 복귀 (슬롯이 감지 못한 경우만)
        if (!eventData.used) // 드롭 슬롯이 처리 안 했으면
        {
            transform.SetParent(originalParent, false);
            transform.localPosition = Vector3.zero;
        }
    }
 }


