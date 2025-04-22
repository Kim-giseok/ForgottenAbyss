using System.Collections;
using System.Collections.Generic;
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

    public void Setup(ShopItemData data, ShopUI shop)
    {
        itemData = data;
        shopUI = shop;
        iconImage.sprite = itemData.item.itemIcon;
        iconImage.enabled = true;

        nameText.text = itemData.item.itemName;
        priceText.text = $"{itemData.price:N0} G";

        GetComponent<Button>().onClick.AddListener(() =>
        {
            shopUI.OnSlotSelected(itemData); // 상위 UI에 알림
        });
    }
}
