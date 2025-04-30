using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public enum SlotMode { Editable, ReadOnly }
public class InventorySlot : SlotBase, IPointerClickHandler
{
    public SlotMode mode = SlotMode.Editable; // 기본값은 일반모드
    private ItemUI itemUI;
    public GameObject equippedOutline; // 장착된 아이템용 외곽선

    private Outline outline;

    private void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>(true);
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        SystemManager.Instance.equipmentManager.OnEquipArmor += OnEquip;
        SystemManager.Instance.equipmentManager.OnUnequipArmor += OnUnequip;
    }

    private void OnDisable()
    {
        SystemManager.Instance.equipmentManager.OnEquipArmor -= OnEquip;
        SystemManager.Instance.equipmentManager.OnUnequipArmor -= OnUnequip;
    }
    // 장비 장착
    private void OnEquip(ArmorSO armor)
    {
        if (currentItem == armor || currentItem is ArmorSO a && a.armorId == armor.armorId)
            equippedOutline?.SetActive(true);
    }

    // 장비 해제
    private void OnUnequip(ArmorSO armor)
    {
        if (currentItem == armor || currentItem is ArmorSO a && a.armorId == armor.armorId)
            equippedOutline?.SetActive(false);
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

        // 장비 장착여부 확인 후 외곽선 표시
        if (item.itemType == ItemType.Equip && item is ArmorSO armor)
        {
            bool isEquipped = SystemManager.Instance.equipmentManager.GetEquippedArmor(armor.slot)?.armorId == armor.armorId;
            equippedOutline?.SetActive(isEquipped);
        }
        else
        {
            equippedOutline?.SetActive(false);
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

                InventoryUIManager.Instance.CheckAndUnequipItem(armor);

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

                InventoryUIManager.Instance.CheckAndUnequipItem(memory);

                if (sys.equipmentManager.IsMemoryPieceEquipped(memorySO.currentMemoryPieceId))
                {
                    sys.equipmentManager.UnequipMemoryPiece();
                    SetOutline(false);
                }
                else
                {
                    sys.equipmentManager.EquipMemoryPiece(memorySO);
                    SetOutline(true, Color.blue);
                }

                break;

            case ItemType.Consumable:
                bool isUsed = currentItem.Use();

                if (isUsed)
                {
                    QuickSlotController.Instance.NotifyItemRemoved(currentItem); // 퀵슬롯에 아이템 삭제 알림           
                    Inventory.Instance.RemoveItemByReference(currentItem);
                    ClearSlot();
                    Inventory.Instance.RefreshInventoryUI();


                }

                break;
        }
    }
}

