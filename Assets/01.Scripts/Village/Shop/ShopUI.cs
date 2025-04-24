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
    [SerializeField] private Color selectedTextColor = Color.white;
    [SerializeField] private Color normalTextColor = Color.black;

    private Button currentSelectedTab;
    private ItemType currentFilterType = ItemType.Equip; // 기본 시작 탭

    private void Start()
    {
        // 탭 번튼에 이벤트 열결
        foreach (var btn in tabButtons)
        {
            Button capturedBtn = btn;
            string tabName = capturedBtn.name;

            capturedBtn.onClick.AddListener(() =>
            {
                OnClickTab(tabName, capturedBtn);
            });
        }

        // 초기화 먼저
        InitializeShop();

        // 기본 첫 탭 선택
        if (tabButtons.Count > 0)
        {
            var firstTab = tabButtons[0];
            OnClickTab(firstTab.name, firstTab);
        }
    }

    private void InitializeShop()
    {
        detailPanel.Hide(); // 상세 패널 초기화
    }

    // itemType과 탭 이름을 동일하게
    private void OnClickTab(string typeStr, Button selectedButton)
    {
        if (Enum.TryParse(typeStr, out ItemType parsedType))
        {
            currentFilterType = parsedType;
            ShowFilteredItems(currentFilterType);
            UpdateTabVisual(selectedButton);
        }
    }

    private void UpdateTabVisual(Button selected)
    {
        foreach (var btn in tabButtons)
        {
            SetButtonStyle(btn, normalColor, normalTextColor);
        }

        SetButtonStyle(selected, selectedColor, selectedTextColor);
        currentSelectedTab = selected;
    }

    // 탭의 배경색, 텍스트색 담당
    private void SetButtonStyle(Button btn, Color backgroundColor, Color textColor)
    {
        var colors = btn.colors;
        colors.normalColor = backgroundColor;
        colors.highlightedColor = backgroundColor;
        colors.pressedColor = backgroundColor;
        colors.selectedColor = backgroundColor;
        btn.colors = colors;

        var text = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.color = textColor;
        }

    }

    private void ShowFilteredItems(ItemType type)
    {
        // 기존 슬롯 숨김
        foreach (Transform child in slotParent)
        {
            child.gameObject.SetActive(false);
        }

        // 타입에 맞는 아이템만 슬롯에 표시
        int activeIndex = 0;
        foreach (var itemData in shopItems)
        {
            if (itemData.item.itemType != type)
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
            slotUI.Setup(itemData, this);

            activeIndex++;
        }
    }

    // 슬롯에서 호출됨
    public void OnSlotSelected(ShopItemData selectedData)
    {
        detailPanel.Show(selectedData);
    }
}
