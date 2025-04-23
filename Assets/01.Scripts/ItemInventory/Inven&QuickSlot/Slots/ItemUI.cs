using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; // 아이템 데이터
    private bool isDraggable = true;
    private CanvasGroup canvasGroup; // 드래그할 때 UI 투명도 설정
    private RectTransform rectTransform; // 드래그 UI 위치 설정
    private Image iconImage;

    private Transform originalParent; // 원래 부모 저장
    private Canvas dragCanvas; // 가장 위에 있는 Canvas

    private bool wasDropped = false; // 드롭 성공 여부

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        iconImage = GetComponent<Image>(); // 아이콘 변경용

        dragCanvas = GameObject.Find("DragCanvas")?.GetComponent<Canvas>(); // 드래그용
    }

    public void SetDraggable(bool canDrag)
    {
        isDraggable = canDrag;
    }

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        item = newItem;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;
    }

    // 아이템 제거
    public void RemoveItem()
    {
        item = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("드래그 시작됨");
        wasDropped = false; // 시작할때 초기화
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
        Debug.Log("드래그 끝남");
        // transform.SetParent(originalParent, true); // 다시 원래 부모로
        canvasGroup.alpha = 1f;   // 드래그 끝나고 원래 상태로
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            var dropHandler = eventData.pointerEnter.GetComponentInParent<SlotBase>();
            if (dropHandler != null)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        // 드롭 실패 시 원래 위치로
        transform.SetParent(originalParent, true);
    }
}


