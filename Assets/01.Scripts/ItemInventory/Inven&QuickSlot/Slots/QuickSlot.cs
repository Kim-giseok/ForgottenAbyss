using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : SlotBase
{
    [SerializeField] private GameObject outlineObject; // 선택된 슬롯 테두리
    [SerializeField] private Image cooldownOverlay; // UI 위에 덮이는 반투명 이미지
    [SerializeField] private float cooldownTime = 3f;

    private float remainingCooldown = 0f;
    private int slotIndex = -1;

    private void Update()
    {
        if (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            cooldownOverlay.fillAmount = remainingCooldown / cooldownTime;
        }
    }

    public void SetIndex(int index)
    {
        slotIndex = index;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<ItemUI>();
        if (dragged != null && dragged.item != null)
        {
            SetItem(dragged.item);
        }
    }

    public override void SetItem(Item item)
    {
        currentItem = item;

        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;

        var itemUI = GetComponentInChildren<ItemUI>(true);
        if (itemUI != null)
        {
            itemUI.gameObject.SetActive(true);
            itemUI.SetItem(item);
            itemUI.SetDraggable(true); // 드래그 가능하게 설정
        }
    }

    public void UseItem()
    {
        if (currentItem != null && remainingCooldown <= 0)
        {
            currentItem.Use(); // 아이템 효과 실행
            remainingCooldown = cooldownTime;

            StartCoroutine(BlinkIcon());
        }

    }

    public void SetSelected(bool selected)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(selected);
        }
    }

    // 슬롯 깜빡임
    private IEnumerator BlinkIcon()
    {
        Image icon = GetComponent<Image>();
        for (int i = 0; i < 4; i++)
        {
            icon.enabled = false;
            yield return new WaitForSeconds(0.1f);
            icon.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
