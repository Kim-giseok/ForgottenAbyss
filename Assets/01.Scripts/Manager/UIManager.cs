using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InventoryUI inventoryUI;  // 인벤토리
    public SettingsMenu settingsMenu; // 옵션창
    public WeaponSwapper weaponSwapper;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))  // 인벤토리 열기/닫기
        {
            inventoryUI.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // 옵션창 열기/닫기
        {
            settingsMenu.ToggleSettingsMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) // 무기 스왑
        {
            weaponSwapper.SwapWeapons();
        }

    }
}
