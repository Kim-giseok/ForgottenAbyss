using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDetailPanel : MonoBehaviour
{
    [Header("Inventory 연결")]
    [SerializeField] private MonoBehaviour containerObject;
    private IItemContainer container => containerObject as IItemContainer;

    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button buyButton;

    private ShopItemData currentData;

    private void Awake()
    {
        if (containerObject == null)
        {
            containerObject = FindObjectOfType<InventoryController>();
            Debug.LogWarning($"[ShopDetailPanel] 자동으로 InventoryController 연결됨: {containerObject?.gameObject.name}");
        }
    }

    public void Show(ShopItemData data)
    {
        if (data == null || data.item == null)
        {
            Debug.LogError("[Shop] 잘못된 ShopItemData 전달됨");
            Hide();
            return;
        }

        currentData = data;

        nameText.text = data.item.itemName;
        descText.text = data.item.itemDescription;
        priceText.text = $"{data.price:N0} G";
        iconImage.sprite = data.item.itemIcon;
        iconImage.enabled = true;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Buy()
    {
        if (!GoldManager.Instance.SpendGold(currentData.price))
        {
            Debug.Log("골드 부족!");
            return;
        }

        var inventory = UIManager.Instance.inventoryUI.InventoryController;
        var quickSlot = UIManager.Instance.quickSlotController;

        int linkedSlotIndex = quickSlot.GetLinkedInventorySlotIndex(currentData.item);
        if (linkedSlotIndex >= 0)
        {
            var slot = inventory.GetSlot(linkedSlotIndex) as Slot;
            if (slot != null)
            {
                slot.Add(1);
                inventory.NotifyChanged();
            }
        }
        else
        {
            inventory.AddItem(currentData.item, 1);
            inventory.NotifyChanged();
        }

        Debug.Log("구매 완료!");
        Hide();
    }
}
