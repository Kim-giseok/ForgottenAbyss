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
        if(slot.Item is ArmorSO armor)
        {
            if (IsEmpty || !SystemManager.Instance.equipmentManager.GetEquippedArmor(armor.slot))
            {
                equippedOutline?.SetActive(false);
                return;
            }
        }
  
        var em = SystemManager.Instance?.equipmentManager;
        bool equipped = slot.Item switch
        {
            ArmorSO armors => em.IsEquipped(armors),
            MemorySkillItem memory => em.IsEquipped(memory),
            _ => false
        };

        equippedOutline?.SetActive(equipped);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;

        var em = SystemManager.Instance.equipmentManager;

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
                    if (em != null && slot.Item is ArmorSO armor)
                    {
                        var equippedSlot = em.GetEquippedArmorSlot(armor.slot);
                        if (equippedSlot == this)
                        {
                            em.UnequipArmor(armor.slot); // 현재 장착 중인 경우 해제
                            Debug.Log($"[InventorySlotUI] {armor.itemName} 장착 해제");
                        }
                        else
                        {
                            var currentlyEquipped = em.GetEquippedArmor(armor.slot);

                            if (currentlyEquipped != null && currentlyEquipped.slot == armor.slot)
                            {
                                em.UnequipArmor(armor.slot); // 기존 장비 해제
                                Debug.Log($"[InventorySlotUI] 기존 {currentlyEquipped.itemName} 해제 후 {armor.itemName} 장착");
                            }

                            em.EquipArmor(armor, this); // 새 장비 장착
                            Debug.Log($"[InventorySlotUI] {armor.itemName} 장착됨");
                        }

                        RefreshOutline();
                    }
                    break;
                case ItemType.Memory:

                    break;
            }
            
        }
    }
}
