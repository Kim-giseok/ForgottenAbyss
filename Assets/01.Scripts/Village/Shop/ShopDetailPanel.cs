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
        Debug.Log($"[Buy] item: {currentData?.item}, name: {currentData?.item?.itemName}");

        if (currentData == null || currentData.item == null)
        {
            Debug.LogError("[Shop] 구매할 아이템 정보가 null입니다.");
            return;
        }

        if (container == null)
        {
            Debug.LogError("[Shop] IItemContainer 연결이 되어있지 않습니다.");
            return;
        }

        if (GoldManager.Instance.SpendGold(currentData.price))
        {
            container.AddItem(currentData.item, 1);
            Debug.Log("구매 완료!");

            //// 인벤토리 열려 있으면 강제로 UI 갱신
            //if (UIManager.Instance.inventoryUI.inventoryPanel.activeSelf)
            //{
            //    UIManager.Instance.inventoryUI.UpdateUI();
            //}

            Hide();
        }
        else
        {
            Debug.Log("골드 부족!");
        }
        
    }
}
