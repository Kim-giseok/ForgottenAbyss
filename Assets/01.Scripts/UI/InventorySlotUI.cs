using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject equippedOutline;
    [SerializeField] private ItemUI itemUI;

    public ISlot Slot => slot;
    public IItemContainer Container => container;
    public int Index { get; private set; }

    public bool IsEmpty => slot == null || slot.Item == null || slot.Quantity <= 0;


    private ISlot slot;
    private IItemContainer container;

    public void SetSlot(ISlot newSlot, int index, IItemContainer parent)
    {
        //Debug.Log($"[InventorySlotUI] SetSlot 호출됨 - Index: {index}, Item: {(newSlot?.Item == null ? "null" : newSlot.Item.itemName)}");

        slot = newSlot;
        Index = index;
        container = parent;

        UpdateUI();

        // Debug.Log($"[InventorySlotUI] SetSlot {index}: {slot?.Item?.itemName ?? "없음"}, 수량: {slot?.Quantity}");
    }

    private void UpdateUI()
    {
        if (IsEmpty)
        {
            iconImage.enabled = false;
            iconImage.sprite = null;
            amountText.text = "";
            // itemUI?.RemoveItem();
            equippedOutline?.SetActive(false);
            return;
        }

        iconImage.sprite = slot.Item.itemIcon;
        iconImage.enabled = true;
        amountText.text = slot.Quantity > 1 ? slot.Quantity.ToString() : "";

        itemUI?.SetItem(slot.Item);
        Debug.Log($"[InventorySlotUI] UpdateUI - 아이템: {slot.Item?.itemName}, IsEmpty: {IsEmpty}");
        RefreshOutline();
    }

    public void Clear()
    {
        iconImage.enabled = false;
        iconImage.sprite = null;
        amountText.text = "";
        equippedOutline?.SetActive(false);
        itemUI?.RemoveItem();

        // 내부 상태도 초기화
        slot = null;
        container = null;
        Index = -1;
    }

    public void RefreshOutline()
    {
        var em = SystemManager.Instance.equipmentManager;

        bool equipped = slot.Item switch
        {
            ArmorSO armors => em.IsEquipped(armors),
            MemorySkillItem memory => em.IsEquipped(memory),
            _ => false
        };

        equippedOutline.SetActive(equipped);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;
        if (SystemManager.Instance == null) return;

        var sys = SystemManager.Instance;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (slot?.Item == null)
            {
                Debug.LogWarning("[InventorySlotUI] 우클릭했지만 slot.Item이 null입니다.");
                return;
            }

            switch (slot.Item.itemType)
            {
                case ItemType.Consumable:
                    string itemName = slot.Item.itemName;

                    Debug.Log($"[InventorySlotUI] {itemName} 우클릭 → 사용 시도");

                    if (itemName == "기억의 파편") break;

                    if (SlotUtils.TryUseSlot(slot))
                    {
                        Debug.Log($"[InventorySlotUI] {itemName} 사용됨");
                        UpdateUI();
                    }
                    else
                    {
                        Debug.LogWarning($"[InventorySlotUI] {itemName} 사용 실패 (조건 불만족?)");
                    }

                    break;

                case ItemType.Equip:
                    var armor = slot.Item as ArmorSO;

                    if (sys.equipmentManager.GetEquippedArmorSlot(armor.slot) == this)
                    {
                        sys.equipmentManager.UnequipArmor(armor.slot);
                    }
                    else
                    {
                        sys.equipmentManager.EquipArmor(armor, this);
                    }
                   
                    RefreshOutline();
                    UpdateUI();
                    UIManager.Instance.inventoryUI.CheckAndUnequipItem(armor);

                    break;

                case ItemType.Memory:
                    var memory = slot.Item as MemorySkillItem;
                    var memoryData = sys.dataManager.GetMemoryPieceData(memory.memoryPieceId);
                    var memorySO = sys.dataManager.GetMemoryVisualSO(memoryData.Name);

                    UIManager.Instance.inventoryUI.CheckAndUnequipItem(memory);

                    if (sys.equipmentManager.GetEquippedMemorySlot() == this)
                    {
                        sys.equipmentManager.UnequipMemoryPiece();
                    }
                    else
                    {
                        sys.equipmentManager.EquipMemoryPiece(memorySO, this);
                    }

                    RefreshOutline();
                    UpdateUI();
                    UIManager.Instance.inventoryUI.CheckAndUnequipItem(memory);
                    break;
            }          
        }
    }
}
