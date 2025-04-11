using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private QuickSlot[] quickSlots; // 퀵슬롯 슬롯들
    [SerializeField] private GameObject selectionIndicator; // 선택된 슬롯 표시

    private int selectedIndex = 0;

    void Start()
    {
        SelectSlot(0);  // 시작시 첫 번째 슬롯 선택
    }

    void Update()
    {
        // 1,2번 키로 슬롯 변경
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);

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
        selectionIndicator.transform.position = quickSlots[selectedIndex].transform.position;
    }
}
