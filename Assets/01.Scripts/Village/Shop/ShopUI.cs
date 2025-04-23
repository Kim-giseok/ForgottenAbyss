using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private List<ShopItemData> shopItems; // 상점에 등록된 상품들
    [SerializeField] private Transform slotParent; // 슬롯을 담을 오브젝트
    [SerializeField] private GameObject slotPrefab; // 슬롯 프리펩
    [SerializeField] private ShopDetailPanel detailPanel; //아이템 상세 패널 연결

    [Header ("Tabs")]
    [SerializeField] private List<Button> tabButtons;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    private Button currentSelectedTab;

    private ItemType currentFilterType = ItemType.Equip; // 기본 시작 탭

    private void OnEnable()
    {
        InitializeShop();

        EventSystem.current.SetSelectedGameObject(tabButtons[0].gameObject);
        UpdateTabButtonVisuals();
    }

    // 상점 초기화
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

            UpdateTabButtonVisuals(); // 탭 시각 효과
        }
    }
    // 탭버튼 색상 조절
    private void UpdateTabButtonVisuals()
    {
        foreach (var tab in tabButtons)
        {
            var colors = tab.colors;
            colors.normalColor = normalColor;
            colors.highlightedColor = normalColor;
            colors.pressedColor = normalColor;
            colors.selectedColor = normalColor;
            tab.colors = colors;

            // 텍스트 색상 기본값으로 되돌리기
            var text = tab.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.color = Color.black; // 기본 텍스트 색
            }
        }

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (selectedButton != null)
            {
                var colors = selectedButton.colors;
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
                colors.pressedColor = selectedColor;
                colors.selectedColor = selectedColor;
                selectedButton.colors = colors;

                // 텍스트 색상 강조
                var text = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.color = Color.white; // 강조된 텍스트 색
                }

                currentSelectedTab = selectedButton;
            }
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
