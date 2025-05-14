using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image iconImage;

    private ShopItemData itemData;
    private ShopUI shopUI;

    private Button button;

    public void Setup(ShopItemData data, ShopUI shop)
    {
        itemData = data;
        shopUI = shop;

        // 아이템 null 검사
        if (itemData == null || itemData.item == null)
        {
            Debug.LogError("[ShopSlotUI] 잘못된 아이템 데이터 전달됨. 슬롯 비활성화");
            gameObject.SetActive(false);
            return;
        }

        Debug.Log($"[ShopSlotUI] Setup: {itemData.item.itemName}");

        iconImage.sprite = itemData.item.itemIcon;
        nameText.text = itemData.item.itemName;
        priceText.text = $"{itemData.price:N0} G";
        iconImage.enabled = true;

        // 중복 클릭 방지
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            shopUI.OnSlotSelected(itemData);
        });

        gameObject.SetActive(true);
    }
}
