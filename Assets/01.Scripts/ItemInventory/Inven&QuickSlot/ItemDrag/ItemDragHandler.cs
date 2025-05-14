using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform dragCanvas;
    [SerializeField] private DragItemPool dragItemPool;

    private GameObject dragIcon;
    private DragManager dragManager;

    private IItemContainer originContainer;
    private int originIndex;

    private void Awake()
    {
        if (dragCanvas == null)
        {
            dragCanvas = GameObject.Find("DragCanvas")?.GetComponent<RectTransform>();
            Debug.LogWarning("[ItemDragHandler] dragCanvas 자동 연결됨");
        }

        if (dragItemPool == null)
        {
            dragItemPool = FindObjectOfType<DragItemPool>();
            Debug.LogWarning("[ItemDragHandler] dragItemPool 자동 연결됨");
        }

        if (dragManager == null)
        {
            dragManager = UIManager.Instance?.DragManager;
            Debug.LogWarning("[ItemDragHandler] dragManager 자동 연결됨");
        }
    }

    public void SetOrigin(IItemContainer container, int index)
    {
        originContainer = container;
        originIndex = index;
    }

    public void Initialize(DragManager manager)
    {
        dragManager = manager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragManager == null || dragItemPool == null || dragCanvas == null)
        {
            Debug.LogError("[ItemDragHandler] 필수 참조(dragManager / dragItemPool / dragCanvas)가 설정되지 않았습니다.");
            return;
        }

        // InventorySlotUI 시도
        var invSlotUI = GetComponentInParent<InventorySlotUI>();
        if (invSlotUI != null && !invSlotUI.IsEmpty)
        {
            dragManager.Set(invSlotUI.Slot.Item, invSlotUI.Container, invSlotUI.Index);
        }
        else
        {
            // QuickSlotUI 시도
            var quickSlotUI = GetComponentInParent<QuickSlotUI>();
            if (quickSlotUI != null && !quickSlotUI.IsEmpty)
            {
                dragManager.Set(quickSlotUI.Slot.Item, quickSlotUI.Container, quickSlotUI.Index);
            }
            else
            {
                return; // 드래그할 수 있는 대상이 없음
            }
        }

        // 드래그 아이콘 생성
        dragIcon = dragItemPool.Get();
        dragIcon.transform.SetParent(dragCanvas.transform, false);
        dragIcon.GetComponent<ItemUI>()?.SetItem(dragManager.Item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            dragIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragItemPool.Return(dragIcon);

        if (TryGetTargetSlot(eventData.pointerEnter, out var target, out int index))
        {
            if (dragManager.OriginContainer != target || dragManager.OriginIndex != index)
            {
                dragManager.OriginContainer.RemoveItemAt(dragManager.OriginIndex);
                target.AddItemAt(index, dragManager.Item, 1);
            }
        }

        dragManager.Clear();
    }

    private bool TryGetTargetSlot(GameObject go, out IItemContainer container, out int index)
    {
        var slotUI = go?.GetComponentInParent<InventorySlotUI>();
        if (slotUI != null)
        {
            container = slotUI.Container;
            index = slotUI.Index;
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
