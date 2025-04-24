using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void OnEnable()
    {
        Inventory.Instance.onItemChanged += UpdateUI;
        UpdateUI();
    }

    private void OnDisable()
    {
        Inventory.Instance.onItemChanged -= UpdateUI;
    }

    public void ToggleInventory()
    {
        bool isOpen = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isOpen);

        if (!isOpen)
        {
            UpdateUI();// 열 때 갱신
        }
    }

    public void UpdateUI()
    {
        var items = Inventory.Instance.items;
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
                slots[i].SetItem(items[i]);
            else
                slots[i].ClearSlot();
        }
    }
}
