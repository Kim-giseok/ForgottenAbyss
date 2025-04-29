using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : SlotBase, IPointerClickHandler
{
    [SerializeField] private GameObject outlineObject; // 선택된 슬롯 테두리
    [SerializeField] private Image cooldownOverlay; // UI 위에 덮이는 반투명 이미지
    [SerializeField] private float cooldownTime = 3f;

    public int SlotIndex { get; private set; }

    private float remainingCooldown = 0f;

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
        SlotIndex = index;
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

    public void SetSelected(bool selected)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(selected);
        }
    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
       if (QuickSlotController.Instance.SelectedIndex == SlotIndex)
        {
            UseItem();
        }
        else
        {
            QuickSlotController.Instance.SelectSlotFromOutside(SlotIndex);
        }
    }
}
