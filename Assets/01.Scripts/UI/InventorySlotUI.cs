using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject equippedOutline;
    [SerializeField] private ItemUI itemUI;

    public ISlot Slot => slot;
    public IItemContainer Container => container;
    public int Index { get; private set; }

    //public bool IsEmpty => slot == null || slot.IsEmpty;
    public bool IsEmpty => slot == null || slot.Item == null || slot.Quantity <= 0;

    private ISlot slot;
    private IItemContainer container;

    public void SetSlot(ISlot newSlot, int index, IItemContainer parent)
    {
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

    private void RefreshOutline()
    {
        if (IsEmpty)
        {
            equippedOutline?.SetActive(false);
            return;
        }

        var em = SystemManager.Instance?.equipmentManager;
        bool equipped = slot.Item switch
        {
            ArmorSO armor => em.IsEquipped(armor),
            MemorySkillItem memory => em.IsEquipped(memory),
            _ => false
        };

        equippedOutline?.SetActive(equipped);
    }
}
