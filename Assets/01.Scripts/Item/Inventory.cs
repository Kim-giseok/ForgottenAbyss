using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory invenInstance; // 인벤토리 싱글톤
    
    public delegate void OnSlotCountChange(int val); // 슬롯 개수가 변한 것을 알리기 위한 델리게이트
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> item = new List<Item>();
    
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

    private void Awake()
    {
        if (invenInstance != null)
        {
            Destroy(gameObject);
            return;
        }
        invenInstance = this;
    }

    void Start()
    {
        slotCount = 4; // 초기 슬롯개수
    }

    public bool AddItem(Item _item)
    {
        if(item.Count < slotCount)
        {
            item.Add(_item); // 인벤토리 리스트에 새 아이템 추가
            if(onChangeItem != null)
            onChangeItem.Invoke();
            return true;
        }
        return false;
    }

    // 땅에 있는 아이템 줍기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItem fieldItem =  collision.GetComponent<FieldItem>();
            if(AddItem(fieldItem.GetItem()))
                fieldItem.DestroyItem();
        }
    }
}
