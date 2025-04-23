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

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Buy()
    {
        if (currentData == null)
        {
            Debug.LogError("[Buy] currentData가 null입니다!");
            return;
        }

        if (currentData.item == null)
        {
            Debug.LogError("[Buy] currentData.item이 null입니다!");
            return;
        }

        if (Inventory.Instance == null)
        {
            Debug.LogError("[Buy] Inventory.Instance가 null입니다!");
            return;
        }

        if (GoldManager.Instance == null)
        {
            Debug.LogError("[Buy] GoldManager.Instance가 null입니다!");
            return;
        }

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
