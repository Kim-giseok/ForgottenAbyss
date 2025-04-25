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
        if (GoldManager.Instance.SpendGold(currentData.price))
        {
            Inventory.Instance.AddItem(currentData.item);
            Debug.Log("구매 완료!");

            // 인벤토리 열려 있으면 강제로 UI 갱신
            if (UIManager.Instance.inventoryUI.inventoryPanel.activeSelf)
            {
                UIManager.Instance.inventoryUI.UpdateUI();
            }

            Hide();
        }
        else
        {
            Debug.Log("골드 부족!");
        }
        
    }
}
