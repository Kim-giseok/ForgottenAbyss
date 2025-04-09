using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // 인벤토리 ui
    bool activeInventory = false;

    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }
}
