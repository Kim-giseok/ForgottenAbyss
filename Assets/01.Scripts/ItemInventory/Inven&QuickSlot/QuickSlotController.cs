using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private QuickSlot[] quickSlots; // 퀵슬롯 슬롯들
    private int selectedIndex = 0;

    void Start()
    {
        selectedIndex = -1; // 시작 시 슬롯 선택 안함
    }

    void Update()
    {
        // 1,2번 키로 슬롯 변경
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);

        // 아이템 사용
        if (Input.GetKeyDown(KeyCode.U))
        {
            quickSlots[selectedIndex].UseItem(); // 현재 선택된 슬롯 아이템 사용
        }
    }

    // 슬롯 선택
    void SelectSlot(int index)
    {
        selectedIndex = index;
        
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetSelected(i == selectedIndex);
        }
    }
}
