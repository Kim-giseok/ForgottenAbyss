using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    [SerializeField] private Button closeButton;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        slots = GetComponentsInChildren<InventorySlot>(true).ToList();
    }

    private void Start()
    {
        inventoryPanel.SetActive(false); // √≥¿Ωø° ≤®µŒ±‚
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.onItemChanged += UpdateUI;
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.onItemChanged -= UpdateUI;
    }

    public void ToggleInventory()
    {
        bool isOpen = inventoryPanel.activeSelf;
        Debug.Log($"ToggleInventory »£√‚µ  °Ê «ˆ¿Á ªÛ≈¬: {isOpen}");

        inventoryPanel.SetActive(!isOpen);
        Debug.Log($"inventoryPanel.SetActive({!isOpen}) Ω««‡µ ");

        if (!isOpen)
        {
            UpdateUI();
            Debug.Log("UpdateUI »£√‚µ ");
        }
        //bool isOpen = inventoryPanel.activeSelf;
        //inventoryPanel.SetActive(!isOpen);

        //if (!isOpen) UpdateUI();
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

    public void Close()
    {
        inventoryPanel.SetActive(false);
    }
}
