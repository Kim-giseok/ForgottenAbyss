using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, ITooltipDataProvider, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject equippedOutline;
    [SerializeField] private ItemUI itemUI;
    [SerializeField] private float cooldownTime = 3f;
    private float remainingCooldown = 0f;

    public ISlot Slot => slot;
    public IItemContainer Container => container;
    public int Index { get; private set; }

    public bool IsEmpty => slot == null || slot.Item == null || slot.Quantity <= 0;

    private bool isLinkedToQuickSlot = false;
    private ISlot slot;
    private IItemContainer container;

    public void SetSlot(ISlot newSlot, int index, IItemContainer parent)
    {
        if (slot is Slot oldSlot)
            oldSlot.OnSlotChanged -= UpdateUI;

        slot = newSlot;
        Index = index;
        container = parent;

        if (slot is Slot currentSlot)
            currentSlot.OnSlotChanged += UpdateUI;

        UpdateUI();
    }

    public void SetLinkedToQuickSlot(bool isLinked)
    {
        isLinkedToQuickSlot = isLinked;

        if (iconImage != null)
        {
            Color iconColor = iconImage.color;
            iconColor.a = isLinked ? 0.5f : 1f;
            iconImage.color = iconColor;
        }

        if (amountText != null)
        {
            amountText.alpha = isLinked ? 0.5f : 1f;
        }

        if (itemUI != null)
        {
            var cg = itemUI.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.alpha = isLinked ? 0.5f : 1f;
        }
    }

    private void Update()
    {
        if (remainingCooldown > 0f)
            remainingCooldown -= Time.deltaTime;
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

        amountText.text = slot.Quantity.ToString();

        itemUI?.SetItem(slot.Item);
        Debug.Log($"[InventorySlotUI] UpdateUI - 아이템: {slot.Item?.itemName}, IsEmpty: {IsEmpty}");
        RefreshOutline();

        SetLinkedToQuickSlot(isLinkedToQuickSlot);
    }

    public void Clear()
    {
        if (slot is Slot oldSlot)
            oldSlot.OnSlotChanged -= UpdateUI;

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

                    if (UIManager.Instance.quickSlotController.IsInventorySlotLinked(Index))
                        return;

                    if (itemName == "기억의 파편") break;

                    if (remainingCooldown > 0f)
                    {
                        Debug.LogWarning($"[InventorySlotUI] {itemName} 쿨타임 중이라 사용 불가 ({remainingCooldown:F1}s 남음)");
                        return;
                    }

                    if (SlotUtils.TryUseSlot(slot))
                    {
                        remainingCooldown = cooldownTime;
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

    public ITooltipData GetTooltipData()
    {
        if (IsEmpty) return null;
        return new ItemTooltipData(slot.Item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var tooltipData = GetTooltipData();
        if (tooltipData != null)
            UIManager.Instance.ShowTooltip(tooltipData, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
