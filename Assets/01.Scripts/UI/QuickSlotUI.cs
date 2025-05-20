using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject outlineObject;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private float cooldownTime = 3f;

    private ItemUI itemUI;
    private float remainingCooldown = 0f;
    private bool waitingToClear = false;

    private IItemContainer container;
    private ISlot slot;
    private int index;

    public ISlot Slot => slot;
    public IItemContainer Container => container;
    public int Index => index;
    public bool IsEmpty => slot == null || slot.IsEmpty;

    private void Awake()
    {
        itemUI = GetComponentInChildren<ItemUI>(true);
    }

    public void SetSlot(ISlot newSlot, int slotIndex, IItemContainer parent)
    {
        slot = newSlot;
        index = slotIndex;
        container = parent;

        if (slot != null && !slot.IsEmpty)
        {
            iconImage.sprite = slot.Item.itemIcon;
            iconImage.enabled = true;
            amountText.text = slot.Quantity.ToString();

            //var dragHandler = itemUI.GetComponent<ItemDragHandler>();
            //dragHandler?.SetOrigin(container, index);

            itemUI?.SetItem(slot.Item);
            itemUI?.gameObject.SetActive(true);
        }
        else
        {
            Clear();
        }

        SetSelected(false);
    }

    public void Clear()
    {
        slot = null;
        iconImage.enabled = false;
        iconImage.sprite = null;
        amountText.text = "";
        cooldownOverlay.fillAmount = 0f;

        itemUI?.RemoveItem();
    }

    private void Update()
    {
        if (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            cooldownOverlay.fillAmount = remainingCooldown / cooldownTime;

            if (remainingCooldown <= 0f)
            {
                cooldownOverlay.fillAmount = 0f;
                if (waitingToClear)
                {
                    if (slot != null && !slot.IsEmpty && slot.Item != null)
                    {
                        container?.RemoveItem(slot.Item, 1);
                        UIManager.Instance.inventoryUI.InventoryController.NotifyChanged();
                    }

                    if (slot == null || slot.IsEmpty)
                    {
                        Clear();
                    }
                    else
                    {
                        // 수량이 남아있으면 UI 갱신
                        amountText.text = slot.Quantity.ToString();
                        itemUI?.SetItem(slot.Item);

                        // 슬롯 재연결
                        if (container is QuickSlotController quickSlot)
                        {
                            quickSlot.TryRebindSlotByItemName(index);
                        }
                    }

                    waitingToClear = false;
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (container is QuickSlotController quickSlot)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log($"[QuickSlotUI] 우클릭으로 퀵슬롯 해제 시도 (index: {index})");
                quickSlot.RemoveItemAt(index);
            }
            else
            {
                if (quickSlot.SelectedIndex == index)
                    UseItem();
                else
                    quickSlot.SelectSlotFromOutside(index);
            }
        }
    }


    public void UseItem()
    {
        if (slot == null || slot.IsEmpty || remainingCooldown > 0f)
            return;

        if (SlotUtils.TryUseSlot(slot))
            waitingToClear = true;

        remainingCooldown = cooldownTime;
        StartCoroutine(BlinkIcon());

        amountText.text = slot.Quantity.ToString();
    }

    public void SetSelected(bool selected)
    {
        outlineObject?.SetActive(selected);
    }

    private IEnumerator BlinkIcon()
    {
        for (int i = 0; i < 4; i++)
        {
            iconImage.enabled = false;
            yield return new WaitForSeconds(0.1f);
            iconImage.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty) return;

        var tooltipData = new ItemTooltipData(slot.Item);
        UIManager.Instance.ShowTooltip(tooltipData, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
