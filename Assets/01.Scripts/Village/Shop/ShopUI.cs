using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private List<ShopItemData> shopItems; // 상점에 등록된 상품들
    [SerializeField] private Transform slotParent; // 슬롯을 담을 오브젝트
    [SerializeField] private GameObject slotPrefab; // 슬롯 프리펩
    [SerializeField] private ShopDetailPanel detailPanel; //아이템 상세 패널 연결

    private ItemType currentFilterType = ItemType.Equip; // 기본 필터 타입

    private void OnEnable() => InitializeShop();

    private void InitializeShop()
    {
        ShowFilteredItems(currentFilterType);
        detailPanel.Hide();

    }

    public void OnClickTab(string typeStr)
    {
        if (Enum.TryParse(typeStr, out ItemType parsedType))
        {
            currentFilterType = parsedType;
            ShowFilteredItems(parsedType);
        }
    }

    private void ShowFilteredItems(ItemType type)
    {
        // 기존 슬롯 비활성화만
        foreach (Transform child in slotParent)
        {
            child.gameObject.SetActive(false);
        }

        // 타입에 맞는 아이템만 슬롯에 표시
        int activeIndex = 0;
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].item.itemType != type)
                continue;

            GameObject slotGO;
            if (activeIndex < slotParent.childCount)
            {
                slotGO = slotParent.GetChild(activeIndex).gameObject;
                slotGO.SetActive(true);
            }
            else
            {
                slotGO = Instantiate(slotPrefab, slotParent);
            }

            var slotUI = slotGO.GetComponent<ShopSlotUI>();
            slotUI.Setup(shopItems[i], this);

            activeIndex++;
        }
    }

    // 슬롯에서 호출됨
    public void OnSlotSelected(ShopItemData selectedData)
    {
        detailPanel.Show(selectedData);
    }
}
