using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDetailPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button buyButton;

    private ShopItemData currentData;

    public void Show(ShopItemData data)
    {
        currentData = data;

        nameText.text = data.item.itemName;
        descText.text = data.item.itemDescription;
        priceText.text = $"{data.price:N0} G";
        iconImage.sprite = data.item.itemIcon;
        iconImage.enabled = true;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);
        Debug.Log("[ShopDetailPanel] 구매 버튼에 리스너 등록됨");

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Buy()
    {
        Debug.Log("[ShopDetailPanel] Buy 호출됨!");
        if (GoldManager.Instance.SpendGold(currentData.price))
        {
            Inventory.Instance.AddItem(currentData.item);
            Debug.Log("구매 완료!");
            Hide();
        }
        else
        {
            Debug.Log("골드 부족!");
        }
    }
}
