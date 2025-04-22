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

    void Awake() { Debug.Log($"{gameObject.name} - Awake"); }
    void Start() { Debug.Log($"{gameObject.name} - Start"); }
    void OnEnable() { Debug.Log($"{gameObject.name} - OnEnable"); }
    void OnDisable() { Debug.Log($"{gameObject.name} - OnDisable"); }

    public void Setup(ShopItemData data, ShopUI shop)
    {
        itemData = data;
        shopUI = shop;

        Debug.Log($"[ShopSlotUI] Setup 호출됨: {itemData.item.itemName}");

        iconImage.sprite = itemData.item.itemIcon;
        nameText.text = itemData.item.itemName;
        priceText.text = $"{itemData.price:N0} G";

        // 상태 출력
        Debug.Log($"[ShopSlotUI] nameText active? {nameText.gameObject.activeSelf}");
        Debug.Log($"[ShopSlotUI] iconImage active? {iconImage.gameObject.activeSelf}");

        // 강제 활성화
        nameText.gameObject.SetActive(true);
        priceText.gameObject.SetActive(true);
        iconImage.enabled = true;

        nameText.text = itemData.item.itemName;
        priceText.text = $"{itemData.price:N0} G";

        GetComponent<Button>().onClick.AddListener(() =>
        {
            shopUI.OnSlotSelected(itemData); // 상위 UI에 알림
        });
    }
}
