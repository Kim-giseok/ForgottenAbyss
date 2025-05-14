using System;
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
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].Set(item, amount);
                slotUIs[i].SetSlot(slots[i], i, this);
                OnContainerChanged?.Invoke();
                return true;
            }
        }

        Debug.LogWarning("[QuickSlotController] 슬롯이 모두 찼습니다.");
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
        slots[index].Clear();
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
}
