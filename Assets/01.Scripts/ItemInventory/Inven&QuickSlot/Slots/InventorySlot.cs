using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public enum SlotMode { Editable, ReadOnly }
public class InventorySlot : SlotBase, IPointerClickHandler
{
    public SlotMode mode = SlotMode.Editable; // 기본값은 일반모드
    private ItemUI itemUI;

    private Outline outline;

    private void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>(true);
        outline = GetComponent<Outline>();
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

    public void SetOutline(bool active, Color color = default)
    {
        if (outline == null)
            return;

        outline.enabled = active;

        if (active)
        {
            outline.effectColor = color == default(Color) ? Color.red : color;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;
        if (SystemManager.Instance == null) return;

        var sys = SystemManager.Instance;

        switch (currentItem.itemType)
        {
            case ItemType.Equip:
                var armor = currentItem as ArmorSO;

                if (sys.equipmentManager.IsArmorEquipped(armor.slot))
                {
                    sys.equipmentManager.UnequipArmor(armor.slot);
                    SetOutline(false);
                }
                else
                {
                    sys.equipmentManager.EquipArmor(armor);
                    SetOutline(true, Color.green);
                }

                break;

            case ItemType.Memory:
                var memory = currentItem as MemorySkillItem;
                var memoryData = sys.dataManager.GetMemoryPieceData(memory.memoryPieceId);
                var memorySO = sys.dataManager.GetMemoryVisualSO(memoryData.Name);

                if (sys.equipmentManager.IsMemoryPieceEquipped(memorySO.currentMemoryPieceId))
                {
                    sys.weaponManager.UnequipMemoryPiece();
                    SetOutline(false);
                }
                else
                {
                    sys.weaponManager.EquipMemoryPiece(memorySO);
                    SetOutline(true, Color.blue);
                }

                break;

            case ItemType.Consumable:
                bool isUsed = currentItem.Use();

                if (isUsed)
                {
                    Inventory.Instance.RemoveItem(currentItem);
                    ClearSlot();
                    Inventory.Instance.RefreshInventoryUI();
                }

                break;
        }
    }
}

