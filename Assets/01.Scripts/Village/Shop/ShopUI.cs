using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private List<ShopItemData> shopItems; // 상점에 등록된 상품들
    [SerializeField] private Transform slotParent; // 슬롯을 담을 오브젝트
    [SerializeField] private GameObject slotPrefab; // 슬롯 프리펩
    [SerializeField] private ShopDetailPanel detailPanel; //아이템 상세 패널 연결

    private void OnEnable() => InitializeShop();

    private void InitializeShop()
    {
        // 기존 슬롯 비활성화만
        foreach (Transform child in slotParent)
        {
            child.gameObject.SetActive(false);
        }

        // 필요한 만큼 새로 생성 or 재활성화
        for (int i = 0; i < shopItems.Count; i++)
        {
            GameObject slotGO;

            if (i < slotParent.childCount)
            {
                slotGO = slotParent.GetChild(i).gameObject;
                slotGO.SetActive(true);
            }
            else
            {
                slotGO = Instantiate(slotPrefab, slotParent);
            }

            var slotUI = slotGO.GetComponent<ShopSlotUI>();
            slotUI.Setup(shopItems[i], this);
        }

        detailPanel.Hide();
    }

    // 슬롯에서 호출됨
    public void OnSlotSelected(ShopItemData selectedData)
    {
        detailPanel.Show(selectedData);
    }
}
