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

    void InitializeShop()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject); // 초기화
        }

        foreach (var item in shopItems)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotParent);
            Debug.Log($"[ShopUI] 슬롯 생성됨: {slotGO.name}");
            var slotUI = slotGO.GetComponent<ShopSlotUI>();
            slotUI.Setup(item, this); // ShopUI를 넘김
        }

        detailPanel.Hide(); // 초기엔 상세 패널 숨김
    }

    // 슬롯에서 호출됨
    public void OnSlotSelected(ShopItemData selectedData)
    {
        detailPanel.Show(selectedData);
    }
}
