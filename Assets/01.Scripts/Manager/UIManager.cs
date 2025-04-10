using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InventoryUI inventoryUI;  // 인벤토리 UI

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))  // 인벤토리 열기/닫기
        {
            inventoryUI.ToggleInventory();
        }
    }
}
