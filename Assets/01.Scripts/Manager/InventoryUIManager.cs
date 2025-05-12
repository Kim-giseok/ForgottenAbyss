using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }

    public GameObject inventoryPanel;
    [SerializeField] private Button closeButton;

    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 슬롯 자동 할당
        slots = GetComponentsInChildren<InventorySlot>(true).ToList();
    }

    private void Start()
    {
        inventoryPanel.SetActive(false); // 시작 시 꺼두기
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        // 이벤트 연결
        StartCoroutine(WaitForInventoryReady());
    }

    private IEnumerator WaitForInventoryReady()
    {
        yield return new WaitUntil(() => Inventory.Instance != null);

        Inventory.Instance.onItemChanged -= UpdateUI; // 중복방지
        Inventory.Instance.onItemChanged += UpdateUI;

        UpdateUI(); // 초기 상태 갱신
    }

    public void ToggleInventory()
    {
        bool isOpen = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isOpen);

        if (!isOpen)
        {
            UpdateUI();
        }
        else
        {
            UIManager.Instance.HideTooltipNextFrame();
        }
    }

    public void Close()
    {
        inventoryPanel.SetActive(false);
        UIManager.Instance.HideTooltipNextFrame(); // 닫을 때 툴팁 제거

    }

    public void UpdateUI()
    {
        if (Inventory.Instance == null) return;

        var items = Inventory.Instance.items;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count && items[i] != null)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

        foreach (var slot in slots)
        {
            slot.RefreshOutline();
        }

        Debug.Log($"[InventoryUIManager] UpdateUI 완료: {items.Count}개 아이템 표시됨");
    }

    public void CheckAndUnequipItem(Item newItem)
    {
        foreach (var slot in slots)
        {
            if (slot.currentItem == null) continue;
            if (slot.currentItem.itemType != newItem.itemType) continue;

            if (newItem.itemType == ItemType.Equip)
            {
                var armor = slot.currentItem as ArmorSO;
                var newArmor = newItem as ArmorSO;

                if (armor != null && newArmor != null && armor.slot == newArmor.slot)
                {
                    slot.RefreshOutline();
                }
            }
            else
            {
                slot.RefreshOutline();
            }
        }
    }
}
