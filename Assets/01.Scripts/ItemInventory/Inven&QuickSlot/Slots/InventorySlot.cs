using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotMode { Editable, ReadOnly }
public class InventorySlot : SlotBase, IPointerClickHandler
{
    public SlotMode mode = SlotMode.Editable; // 기본값은 일반모드
    private ItemUI itemUI;

    [SerializeField] private TextMeshProUGUI amountText; // 아이템 수량 표시

    public GameObject equippedOutline; // 장착된 아이템용 외곽선
    private Outline outline;

    private void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>(true);
    }

    private void OnEnable()
    {
        SystemManager.Instance.equipmentManager.OnEquipArmor += OnEquip;
        SystemManager.Instance.equipmentManager.OnUnequipArmor += OnUnequip;
        SystemManager.Instance.equipmentManager.OnEquipMemory += OnEquipMemory;
        SystemManager.Instance.equipmentManager.OnUnequipMemory += OnUnequipMemory;
    }

    private void OnDisable()
    {
        SystemManager.Instance.equipmentManager.OnEquipArmor -= OnEquip;
        SystemManager.Instance.equipmentManager.OnUnequipArmor -= OnUnequip;
        SystemManager.Instance.equipmentManager.OnEquipMemory -= OnEquipMemory;
        SystemManager.Instance.equipmentManager.OnUnequipMemory -= OnUnequipMemory;

    }
    // 장비 장착
    private void OnEquip(ArmorSO armor)
    {
        if (currentItem == armor || currentItem is ArmorSO a && a.armorId == armor.armorId)
            RefreshOutline();
    }

    // 장비 해제
    private void OnUnequip(ArmorSO armor)
    {
        if (currentItem == armor || currentItem is ArmorSO a && a.armorId == armor.armorId)
            RefreshOutline();
    }

    private void OnEquipMemory(MemoryPieceSO memorySO)
    {
        RefreshOutline();
    }

    private void OnUnequipMemory(MemoryPieceSO memorySO)
    {
        RefreshOutline();
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
            RefreshOutline();
        }
        else
        {
            RefreshOutline();
        }

        UpdateAmount(item.currentAmount);
    }

    private void UpdateAmount(int amount)
    {
        if (amountText != null)
            amountText.text = amount > 1 ? amount.ToString() : "";
    }

    public void RefreshOutline()
    {
        if (currentItem == null)
        {
            equippedOutline?.SetActive(false);
            return;
        }

        switch (currentItem.itemType)
        {
            case ItemType.Equip:
                if (currentItem is ArmorSO armor)
                {
                    bool isEquipped = SystemManager.Instance.equipmentManager.GetEquippedArmorSlot(armor.slot) == this;
                    equippedOutline?.SetActive(isEquipped);
                }
                break;

            case ItemType.Memory:
                {
                    bool isEquipped = SystemManager.Instance.equipmentManager.GetEquippedMemorySlot() == this;
                    equippedOutline?.SetActive(isEquipped);
                }
                break;

            default:
                equippedOutline?.SetActive(false);
                break;
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot(); // 기본 슬롯 초기화
        UpdateAmount(0); // 수량 텍스트 초기화
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

                if (sys.equipmentManager.GetEquippedArmorSlot(armor.slot) == this)
                {
                    sys.equipmentManager.UnequipArmor(armor.slot);
                }
                else
                {
                    sys.equipmentManager.EquipArmor(armor, this);
                }

                break;

            case ItemType.Memory:
                var memory = currentItem as MemorySkillItem;
                var memoryData = sys.dataManager.GetMemoryPieceData(memory.memoryPieceId);
                var memorySO = sys.dataManager.GetMemoryVisualSO(memoryData.Name);

                InventoryUIManager.Instance.CheckAndUnequipItem(memory);

                if (sys.equipmentManager.GetEquippedMemorySlot() == this)
                {
                    sys.equipmentManager.UnequipMemoryPiece();
                }
                else
                {
                    sys.equipmentManager.EquipMemoryPiece(memorySO, this);
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

