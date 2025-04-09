using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory invenInstance; // 인벤도리 싱글톤
    private void Awake()
    {
        if(invenInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        invenInstance = this;
    }

    public delegate void OnSlotCountChange(int val); // 슬롯 개수가 변한 것을 알리기 위한 델리게이트
    public OnSlotCountChange onSlotCountChange;

    private int slotCount; // 슬롯 개수
    public int SlotCount
    {
        get => slotCount;
        set
        {
            slotCount = value;
            onSlotCountChange?.Invoke(slotCount); // 델리게이트 호출
        }
    }

    void Start()
    {
        slotCount = 4; // 초기 슬롯개수
    }
}
