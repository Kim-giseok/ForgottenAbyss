using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : SlotBase
{
    [SerializeField] private GameObject outlineObject; // 선택된 슬롯 테두리
    [SerializeField] private Image cooldownOverlay; // UI 위에 덮이는 반투명 이미지
    [SerializeField] private float cooldownTime = 3f;

    public int SlotIndex { get; private set; }

    private float remainingCooldown = 0f;
    private bool waitingToClear = false; // 쿨타임 끝나면 삭제
    private Item linkedItem; // 인벤토리에서 참조할 아이템

    private void Update()
    {
        if (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            cooldownOverlay.fillAmount = remainingCooldown / cooldownTime;

            if (remainingCooldown <= 0f)
            {
                Debug.Log($"[QuickSlot {SlotIndex}] 쿨타임 종료");
                cooldownOverlay.fillAmount = 0f;

                if (waitingToClear)
                {
                    Debug.Log($"[QuickSlot {SlotIndex}] 대기 중이던 슬롯 비우기 실행");
                    
                    ClearSlot(); // 쿨타임 끝났을 때만 제거
                    waitingToClear = false;
                }
            }
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
            // 드랍할 때 인벤토리에 있는 아이템만 등록
            if (Inventory.Instance.items.Contains(dragged.item))
            {
                SetLinkedItem(dragged.item);
            }
        }
    }

    public void SetLinkedItem(Item item)
    {
        linkedItem = item;
        currentItem = item;
        iconImage.sprite = item.itemIcon;
        iconImage.enabled = true;

        var itemUI = GetComponentInChildren<ItemUI>(true);
        if (itemUI != null)
        {
            itemUI.SetItem(item);
            itemUI.SetDraggable(true);
        }
    }

    public override void SetItem(Item item)
    {
        SetLinkedItem(item);
    }

    public void UseItem()
    {
        if (linkedItem != null && remainingCooldown <= 0)
        {
            bool isUsed = linkedItem.Use();
            if (isUsed)
            {
                if (Inventory.Instance != null)
                {
                    bool removed = Inventory.Instance.RemoveItemByReference(linkedItem);

                    //if (removed)
                    //{
                    //    Debug.Log($"[QuickSlot] 인벤토리에서 '{linkedItem?.name ?? "Unknown"}' 제거됨");
                    //}
                    //else
                    //{
                    //    Debug.LogWarning($"[QuickSlot] 인벤토리에 '{linkedItem.name}' 없음");
                    //}

                    Inventory.Instance.RefreshInventoryUI();
                    QuickSlotController.Instance.NotifyItemRemoved(linkedItem);
                }
                else
                {
                    Debug.LogError("[QuickSlot] Inventory.Instance가 null입니다. 인벤토리 매니저가 없음");
                }

                waitingToClear = true;// 퀵슬롯 자체도 클리어
            }
            remainingCooldown = cooldownTime;

            StartCoroutine(BlinkIcon());
        }

    }

    // 이 슬롯에 해당 아이템이 있는지 확인
    public bool HasItem(Item item)
    {
        return linkedItem == item;
    }

    // 인벤토리에서 아이템 삭제 알림 받으면
    public void MarkToClearAfterCooldown()
    {
        if (linkedItem == null) return;

        // 삭제 요청이 왔을 때
        if (remainingCooldown > 0f)
        {
            waitingToClear = true;
        }
        else
        {
            ClearSlot();
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        linkedItem = null;
        waitingToClear = false;
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
   
}
