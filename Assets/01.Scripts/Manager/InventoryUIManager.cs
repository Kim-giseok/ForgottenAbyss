using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private InventorySlotUI[] slotUIs; // Slot UI 배열
    [SerializeField] private InventoryController inventoryController;

    private void Awake()
    {
        Debug.Log($"[InventoryUIManager] 연결된 inventoryController: {inventoryController?.gameObject.name}");

        // 슬롯 자동 할당
        slotUIs = GetComponentsInChildren<InventorySlotUI>(true);

        if (closeButton != null )
            closeButton.onClick.AddListener(Close);
    }

    private void Start()
    {
        inventoryPanel.SetActive(false);

        if (inventoryController != null)
        {
            inventoryController.OnContainerChanged -= UpdateUI;
            inventoryController.OnContainerChanged += UpdateUI;
            UpdateUI();
        }
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
        Debug.Log("=== [InventoryUIManager] UpdateUI 시작 ===");

        if (inventoryController == null || slotUIs == null) return;

        for (int i = 0; i < slotUIs.Length; i++)
        {
            var slotUI = slotUIs[i];

            if (i < inventoryController.SlotCount)
            {
                var slot = inventoryController.GetSlot(i);
                Debug.Log($"[UpdateUI] [{i}] 아이템: {slot?.Item?.itemName ?? "없음"}, 수량: {slot?.Quantity}, 해시: {slot?.GetHashCode()}");

                slotUI.SetSlot(slot, i, inventoryController); // 슬롯 + 인덱스 + 컨테이너 전달
            }
            else
            {
                slotUI.Clear();
            }
        }
    }

    //public void CheckAndUnequipItem(Item newItem)
    //{
    //    foreach (var slot in slots)
    //    {
    //        if (slot.currentItem == null) continue;
    //        if (slot.currentItem.itemType != newItem.itemType) continue;

    //        if (newItem.itemType == ItemType.Equip)
    //        {
    //            var armor = slot.currentItem as ArmorSO;
    //            var newArmor = newItem as ArmorSO;

    //            if (armor != null && newArmor != null && armor.slot == newArmor.slot)
    //            {
    //                slot.RefreshOutline();
    //            }
    //        }
    //        else
    //        {
    //            slot.RefreshOutline();
    //        }
    //    }
    //}
}
