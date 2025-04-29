using UnityEngine.EventSystems;

public enum SlotMode { Editable, ReadOnly }
public class InventorySlot : SlotBase, IPointerClickHandler
{
    public SlotMode mode = SlotMode.Editable; // 기본값은 일반모드
    private ItemUI itemUI;

    private void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>(true);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item);
        }
    }

    public override void SetItem(Item item)
    {
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;

        if (itemUI != null)
        {
            itemUI.SetItem(item);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        bool isUsed = currentItem.Use();

        if (isUsed)
        {
            Inventory.Instance.RemoveItem(currentItem);
            ClearSlot();
            Inventory.Instance.RefreshInventoryUI();
        }
    }
}

