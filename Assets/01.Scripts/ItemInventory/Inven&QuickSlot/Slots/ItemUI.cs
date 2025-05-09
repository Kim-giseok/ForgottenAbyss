using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; // 아이템 데이터
    private Image image;
    private Transform originalParent; // 원래 부모 저장
    private CanvasGroup canvasGroup; // 드래그할 때 UI 투명도 설정
    private RectTransform rectTransform; // 드래그 UI 위치 설정
    private Canvas dragCanvas; // 가장 위에 있는 Canvas

    private GameObject dragCopy; // 드래그 복제본
    private bool isDraggable = true;


    private void Awake()
    {
        Debug.Log("[ItemUI] Awake 호출");

        rectTransform = GetComponent<RectTransform>();
        dragCanvas = GameObject.Find("DragCanvas")?.GetComponent<Canvas>(); // 드래그용
    }

    // 아이템 설정
    public void SetItem(Item newItem)
    {
        item = newItem;

        if (image == null) image = GetComponent<Image>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        if (image != null) image.sprite = item.itemIcon;
        if (canvasGroup != null) canvasGroup.alpha = 1f;

        SetDraggable(true);
        gameObject.SetActive(true);

    }

    public void SetDraggable(bool canDrag)
    {
        isDraggable = canDrag;
    }

    // 아이템 제거
    public void RemoveItem()
    {
        item = null;

        if (image == null) image = GetComponent<Image>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        if (image != null)
        {
            image.sprite = null;
            image.raycastTarget = true;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = true;
        }

        SetDraggable(false);
    }

    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable || item == null) return;

        // 풀에서 가져오기
        dragCopy = DragItemPool.Instance.Get();
        dragCopy.transform.SetParent(dragCanvas.transform, false);

        var copyItemUI = dragCopy.GetComponent<ItemUI>();
        copyItemUI.SetItem(item);

        dragCopy.transform.position = transform.position;

        var copyCanvasGroup = dragCopy.GetComponent<CanvasGroup>();
        if (copyCanvasGroup != null)
        {
            copyCanvasGroup.alpha = 0.6f;
            copyCanvasGroup.blocksRaycasts = false;
        }
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
                // InventorySlot이면 기존대로 처리
                if (fromSlot is InventorySlot)
                {
                    Inventory.Instance.RemoveItem(fromSlot.currentItem);
                    fromSlot.ClearSlot();
                }
                // QuickSlot이면 UI를 바로 Clear하지 말고 대체 처리
                else if (fromSlot is QuickSlot quickFromSlot)
                {
                    QuickSlotController.Instance.NotifyItemRemoved(fromSlot.currentItem);
                }

                // 드롭 슬롯에 아이템 세팅
                dropSlot.SetItem(item);

                // Inventory에 다시 등록
                if (dropSlot is InventorySlot)
                    Inventory.Instance.AddItem(item);

                // QuickSlot일 경우 퀵슬롯에 아이템 등록
                if (dropSlot is QuickSlot quickDropSlot)
                    QuickSlotController.Instance.SetItemToQuickSlot(quickDropSlot.SlotIndex, item);
            }

        }

        // 드래그 종료 후 항상 인벤토리 UI 갱신
        Inventory.Instance.RefreshInventoryUI();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null && UIManager.Instance.inventoryUI.gameObject.activeSelf)
            UIManager.Instance.ShowTooltip(item, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}


