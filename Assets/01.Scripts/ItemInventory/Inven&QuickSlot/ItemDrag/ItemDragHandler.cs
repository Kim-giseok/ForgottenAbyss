using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform dragCanvas;
    [SerializeField] private DragItemPool dragItemPool;

    private GameObject dragIcon;
    private DragManager dragManager;

    //private IItemContainer originContainer;
    //private int originIndex;

    private void Awake()
    {
        if (dragCanvas == null)
        {
            dragCanvas = GameObject.Find("DragCanvas")?.GetComponent<RectTransform>();
        }

        if (dragItemPool == null)
        {
            dragItemPool = FindObjectOfType<DragItemPool>();
        }
    }

    private void Start()
    {
        if (dragManager == null)
        {
            dragManager = UIManager.Instance?.DragManager;
            Debug.Log("[ItemDragHandler] Start에서 DragManager 재할당 시도: " + dragManager);
        }
    }

    //public void SetOrigin(IItemContainer container, int index)
    //{
    //    originContainer = container;
    //    originIndex = index;
    //}

    public void Initialize(DragManager manager)
    {
        dragManager = manager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragManager == null || dragItemPool == null || dragCanvas == null)
        {
            Debug.LogError("[ItemDragHandler] 필수 참조가 누락되었습니다.");
            return;
        }

        // 드래그 시작 위치 슬롯 감지
        var invSlotUI = GetComponentInParent<InventorySlotUI>();
        var quickSlotUI = GetComponentInParent<QuickSlotUI>();

        if (invSlotUI != null && !invSlotUI.IsEmpty)
        {
            dragManager.Set(invSlotUI.Slot.Item, invSlotUI.Container, invSlotUI.Index);
        }
        else if (quickSlotUI != null && !quickSlotUI.IsEmpty)
        {
            dragManager.Set(quickSlotUI.Slot.Item, quickSlotUI.Container, quickSlotUI.Index);
        }
        else
        {
            Debug.LogWarning("[ItemDragHandler] 드래그 시작 위치에 유효한 슬롯이 없습니다.");
            return;
        }

        // 드래그 아이콘 생성
        dragIcon = dragItemPool.Get();
        dragIcon.transform.SetParent(dragCanvas.transform, false);
        dragIcon.GetComponent<ItemUI>()?.SetItem(dragManager.Item);

        // Raycast 막지 않도록 설정
        var cg = dragIcon.GetComponent<CanvasGroup>();
        if (cg != null) cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            dragIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragItemPool.Return(dragIcon);

        if (!TryGetTargetSlot(eventData, out var targetContainer, out int targetIndex))
        {
            Debug.LogWarning("[ItemDragHandler] 드롭 위치에 유효한 슬롯 없음");
            dragManager.Clear();
            return;
        }

        // 자기 슬롯이면 무시
        if (dragManager.OriginContainer == targetContainer && dragManager.OriginIndex == targetIndex)
        {
            dragManager.Clear();
            return;
        }

        var originSlot = dragManager.OriginContainer.GetSlot(dragManager.OriginIndex);

        if (dragManager.OriginContainer == targetContainer)
        {
            // 같은 컨테이너 → 스왑
            dragManager.OriginContainer.SwapItems(dragManager.OriginIndex, targetIndex);
        }
        else
        {
            // 다른 컨테이너 간 이동 시 반드시 미리 복사한 값을 사용
            var draggedItem = originSlot.Item;
            var draggedAmount = originSlot.Quantity;

            dragManager.OriginContainer.RemoveItemAt(dragManager.OriginIndex);
            targetContainer.AddItemAt(targetIndex, draggedItem, draggedAmount);
        }

        dragManager.Clear();
    }

    private bool TryGetTargetSlot(PointerEventData eventData, out IItemContainer container, out int index)
    {
        // 1차: pointerEnter 기준
        if (TryExtractContainer(eventData.pointerEnter, out container, out index))
            return true;

        // 2차: hovered 리스트 기준 (더 정밀하게 감지)
        foreach (var hovered in eventData.hovered)
        {
            if (TryExtractContainer(hovered, out container, out index))
                return true;
        }

        container = null;
        index = -1;
        return false;
    }

    private bool TryExtractContainer(GameObject go, out IItemContainer container, out int index)
    {
        var invSlotUI = go?.GetComponentInParent<InventorySlotUI>();
        if (invSlotUI != null)
        {
            container = invSlotUI.Container;
            index = invSlotUI.Index;
            return true;
        }

        var quickSlotUI = go?.GetComponentInParent<QuickSlotUI>();
        if (quickSlotUI != null)
        {
            container = quickSlotUI.Container;
            index = quickSlotUI.Index;
            return true;
        }

        container = null;
        index = -1;
        return false;
    }
}
