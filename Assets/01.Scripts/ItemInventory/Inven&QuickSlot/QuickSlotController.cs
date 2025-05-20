using System;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour, IItemContainer
{
    [SerializeField] private QuickSlotUI[] slotUIs;
    [SerializeField] private int slotCount = 5;

    private Slot[] slots;
    public int SelectedIndex { get; private set; } = -1;

    public event Action OnContainerChanged;
    public event Action<Item> OnItemRemovedExternally;
    public int SlotCount => slots.Length;

    private Dictionary<int, int> linkedInventorySlots = new();
    private Dictionary<int, string> quickSlotItemNames = new();

    private void Awake()
    {
        slots = new Slot[slotCount];
        for (int i = 0; i < slotCount; i++)
            slots[i] = new Slot();

        for (int i = 0; i < slotUIs.Length; i++)
            slotUIs[i].SetSlot(slots[i], i, this);
    }

    public void OnQuickSlotKeyPressed(int index)
    {
        if (index < 0 || index >= slotUIs.Length) return;

        if (SelectedIndex == index)
            slotUIs[index].UseItem();
        else
            SelectSlot(index);
    }

    // 마우스 클릭할때 사용
    public void SelectSlotFromOutside(int index)
    {
        SelectSlot(index);
    }

    // 슬롯 선택
    private void SelectSlot(int index)
    {
        SelectedIndex = index;

        for (int i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].SetSelected(i == index);
        }
    }

    public bool AddItem(Item item, int amount)
    {
        //for (int i = 0; i < slots.Length; i++)
        //{
        //    if (slots[i].IsEmpty)
        //    {
        //        slots[i].Set(item, amount);
        //        slotUIs[i].SetSlot(slots[i], i, this);
        //        OnContainerChanged?.Invoke();
        //        return true;
        //    }
        //}

        Debug.LogError("[QuickSlotController] 직접 AddItem()은 허용되지 않음. 반드시 LinkToInventorySlot() 사용");
        return false;
    }

    public bool AddItemAt(int index, Item item, int amount)
    {
        if (index < 0 || index >= slots.Length) return false;

        slots[index].Set(item, amount);
        slotUIs[index].SetSlot(slots[index], index, this);
        OnContainerChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(Item item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == item)
            {
                slots[i].Clear();
                slotUIs[i].Clear();
                OnContainerChanged?.Invoke();
                return true;
            }
        }

        return false;
    }

    public bool RemoveItemAt(int index)
    {
        if (index < 0 || index >= slots.Length) return false;

        if (linkedInventorySlots.ContainsKey(index))
            linkedInventorySlots.Remove(index);

        slots[index].Clear();
        slotUIs[index].Clear();
        OnContainerChanged?.Invoke();
        return true;
    }


    public bool SwapItems(int indexA, int indexB)
    {
        if (indexA == indexB || indexA >= slots.Length || indexB >= slots.Length) return false;

        (slots[indexA], slots[indexB]) = (slots[indexB], slots[indexA]);

        slotUIs[indexA].SetSlot(slots[indexA], indexA, this);
        slotUIs[indexB].SetSlot(slots[indexB], indexB, this);
        OnContainerChanged?.Invoke();
        return true;
    }

    public bool CanAccept(Item item)
    {
        return item.itemType == ItemType.Consumable;
    }

    public ISlot GetSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return null;
        return slots[index];
    }

    public void NotifyItemRemoved(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == item)
            {
                slots[i].Clear();
                slotUIs[i].Clear();
            }
        }

        OnItemRemovedExternally?.Invoke(item);
    }

    public int GetLinkedInventorySlotIndex(Item item)
    {
        foreach (var pair in linkedInventorySlots)
        {
            int inventoryIndex = pair.Value;
            var slot = UIManager.Instance.inventoryUI.InventoryController.GetSlot(inventoryIndex);

            if (!slot.IsEmpty && slot.Item == item)
                return inventoryIndex;
        }

        return -1;
    }

    public void LinkToInventorySlot(int quickSlotIndex, int inventorySlotIndex, ISlot inventorySlot, IItemContainer inventory)
    {
        slots[quickSlotIndex] = inventorySlot as Slot;
        linkedInventorySlots[quickSlotIndex] = inventorySlotIndex;

        if (inventorySlot.Item != null)
            quickSlotItemNames[quickSlotIndex] = inventorySlot.Item.itemName;

        slotUIs[quickSlotIndex].SetSlot(inventorySlot, quickSlotIndex, inventory);
        OnContainerChanged?.Invoke();
    }

    public void TryRebindSlotByItemName(int quickSlotIndex)
    {
        if (!quickSlotItemNames.TryGetValue(quickSlotIndex, out string itemName)) return;

        var inventory = UIManager.Instance.inventoryUI.InventoryController;

        for (int i = 0; i < inventory.SlotCount; i++)
        {
            var slot = inventory.GetSlot(i);
            if (!slot.IsEmpty && slot.Item.itemName == itemName)
            {
                LinkToInventorySlot(quickSlotIndex, i, slot, inventory);
                Debug.Log($"[QuickSlot] 슬롯 {quickSlotIndex} 다시 연결됨 → 인벤토리 슬롯 {i}");
                return;
            }
        }

        Debug.LogWarning($"[QuickSlot] {itemName} 해당 인벤토리 슬롯 못 찾음 → 재연결 실패");
    }

    public bool IsInventorySlotLinked(int inventoryIndex)
    {
        return linkedInventorySlots.ContainsValue(inventoryIndex);
    }

    public void UnlinkInventorySlot(int inventoryIndex)
    {
        foreach (var pair in linkedInventorySlots)
        {
            linkedInventorySlots.Remove(pair.Key);
            break;
        }
    }
}
