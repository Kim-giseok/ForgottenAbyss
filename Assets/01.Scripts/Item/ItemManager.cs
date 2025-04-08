using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Item[] allItems;  // 게임 내에서 사용할 아이템들

    public Item GetItem(int index)
    {
        return allItems[index];
    }

    public void AddItemToSlot(Item item, int slotIndex, QuickSlotController quickSlotController)
    {
        quickSlotController.AddItemToSlot(item, slotIndex);
    }
}
