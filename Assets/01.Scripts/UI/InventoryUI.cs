using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel; // 인벤토리 ui
    public Button closeButton; // 인벤 닫기 버튼
    bool activeInventory = false;

    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
        closeButton.onClick.AddListener(CloseInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ActiveInventory();
    }

    // 인벤 활성화/ 비활성화 함수
    void ActiveInventory()
    {
        activeInventory = !activeInventory;
        inventoryPanel.SetActive(activeInventory);
    }

    // 인벤 닫기 ㅎ함수
    void CloseInventory()
    {
        activeInventory = false;
        inventoryPanel.SetActive(activeInventory);
    }
}
