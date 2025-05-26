using System;
using UnityEngine;

public class InventoryController : MonoBehaviour, IItemContainer
{
    [SerializeField] private int slotSize = 20;

    private Slot[] slots;

    public event Action OnContainerChanged;

    public int SlotCount => slots.Length;

    private void InitSlots()
    {
        slots = new Slot[slotSize];
        for (int i = 0; i < slotSize; i++)
            slots[i] = new Slot();
    }

    private void Awake()
    {
        Debug.Log($"[InventoryController] Awake called on GameObject: {gameObject.name}, ID: {GetInstanceID()}");
        if (slots == null || slots.Length != slotSize)
        {
            InitSlots();
            Debug.Log("[InventoryController] 슬롯 배열 초기화 완료");
        }
    }

    private void OnEnable()
    {
        if (slots == null || slots.Length != slotSize)
        {
            InitSlots();
            Debug.Log("[InventoryController] OnEnable에서 슬롯 배열 초기화");
        }
    }

    private void EnsureInitialized()
    {
        if (slots == null || slots.Length != slotSize)
        {
            InitSlots();
            Debug.Log("[InventoryController] 슬롯 배열 강제 초기화");
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = new Slot();
                Debug.Log($"[InventoryController] 슬롯 {i}이 null이라 새로 생성함");
            }
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if (item == null)
        {
            Debug.LogError("[InventoryController] AddItem()에 null 아이템 전달됨!");
            return false;
        }

        EnsureInitialized();

        Debug.Log($"[InventoryController] AddItem() 호출됨 on ID: {GetInstanceID()}");
        if (slots == null)
        {
            Debug.LogError("[InventoryController] 슬롯 배열이 초기화되지 않았습니다. GameObject가 비활성 상태였을 수 있습니다.");
            return false;
        }
        Debug.Log($"[InventoryController] AddItem: {item.itemName}, amount: {amount}, maxStack: {item.maxStack}");

        bool changed = false;

        // 스택 가능한 슬롯 찾기
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"[Inventory] Slot {i} 상태: {slots[i].Item?.name ?? "비어있음"}, 수량: {slots[i].Quantity}");
            var slot = slots[i];
            if (!slot.IsEmpty && slot.CanStack(item))
            {
                int space = item.maxStack - slot.Quantity;
                int toAdd = Mathf.Min(space, amount);

                slot.Add(toAdd);
                amount -= toAdd;
                changed = true;

                if (amount <= 0)
                    break;
            }
        }

        // 빈 슬롯에 추가
        for (int i = 0; i < slots.Length && amount > 0; i++)
        {
            var slot = slots[i];
            if (slot.IsEmpty)
            {
                int toAdd = Mathf.Min(item.maxStack, amount);
                slot.Set(item, toAdd);
                amount -= toAdd;
                changed = true;

                if (amount <= 0)
                    break;
            }
        }

        if (changed)
        {
            Debug.Log("[InventoryController] OnContainerChanged 호출");
            OnContainerChanged?.Invoke();
        }

        return amount <= 0;
    }

    public bool AddItemAt(int index, Item item, int amount)
    {
        if (index < 0 || index >= slots.Length)
            return false;

        var slot = slots[index];
        bool changed = false;

        if (slot.IsEmpty)
        {
            int toAdd = Mathf.Min(item.maxStack, amount);
            slot.Set(item, toAdd);
            changed = true;
        }
        else if (slot.Item == item && slot.CanStack(item))
        {
            int space = item.maxStack - slot.Quantity;
            int toAdd = Mathf.Min(space, amount);
            slot.Add(toAdd);
            changed = true;
        }
        else
        {
            Debug.LogWarning($"[Inventory] 슬롯 {index}은 비어있지 않거나 다른 아이템이 있습니다.");
            return false;
        }

        if (changed)
            OnContainerChanged?.Invoke();

        return true;
    }


    public bool RemoveItem(Item item, int amount)
    {
        int remaining = amount;
        bool changed = false;

        for (int i = 0; i < slots.Length && remaining > 0; i++)
        {
            var slot = slots[i];
            if (slot.IsEmpty || slot.Item != item)
                continue;

            if (slot.Quantity > remaining)
            {
                slot.Remove(remaining);
                remaining = 0;
                changed = true;
            }
            else
            {
                remaining -= slot.Quantity;
                slot.Clear();
                changed = true;
            }
        }

        if (changed)
            OnContainerChanged?.Invoke();

        if (remaining > 0)
        {
            Debug.LogWarning($"[Inventory] '{item.itemName}' 수량 부족: {amount} 요청, {amount - remaining}만 제거됨");
            return false;
        }

        return true;
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
        if (indexA < 0 || indexA >= slots.Length || indexB < 0 || indexB >= slots.Length)
            return false;

        if (indexA == indexB)
            return false;

        var itemA = slots[indexA].Item;
        var itemB = slots[indexB].Item;

        if (UIManager.Instance.quickSlotController.IsInventorySlotLinked(indexA) ||
            UIManager.Instance.quickSlotController.IsInventorySlotLinked(indexB))
        {
            if (itemA != null && itemB != null && itemA.itemType == ItemType.Consumable && itemB.itemType == ItemType.Consumable)
            {
                Debug.LogWarning("[SwapItems] 퀵슬롯에 등록된 아이템을 다른 소비 아이템으로 변경할 수 없습니다.");
                return false;
            }

            Debug.LogWarning("[SwapItems] 퀵슬롯에 등록된 아이템은 스왑할 수 없습니다.");
            return false;
        }


        if ((itemA is ArmorSO armorA && SystemManager.Instance.equipmentManager.IsSpecificArmorEquipped(armorA)) ||
            (itemB is ArmorSO armorB && SystemManager.Instance.equipmentManager.IsSpecificArmorEquipped(armorB)) ||
            (itemA is MemorySkillItem memoryA && SystemManager.Instance.equipmentManager.IsMemoryPieceEquipped(memoryA.memoryPieceId)) ||
            (itemB is MemorySkillItem memoryB && SystemManager.Instance.equipmentManager.IsMemoryPieceEquipped(memoryB.memoryPieceId)))
        {
            Debug.LogWarning("[SwapItems] 장착된 아이템(방어구 또는 기억 조각)은 스왑할 수 없습니다.");
            return false;
        }

        var tempItem = itemA;
        var tempAmount = slots[indexA].Quantity;

        slots[indexA].Set(slots[indexB].Item, slots[indexB].Quantity);
        slots[indexB].Set(tempItem, tempAmount);

        OnContainerChanged?.Invoke();
        return true;
    }


    public bool CanAccept(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];

            if (slot.IsEmpty)
                return true;

            if (slot.Item == item && slot.CanStack(item))
                return true;
        }

        return false;
    }

    public ISlot GetSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogWarning($"[Inventory] 슬롯 인덱스 범위 초과: {index}");
            return null;
        }

        return slots[index];
    }

    public int FindFirstSlotIndex(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty && slots[i].Item == item)
                return i;
        }
        return -1;
    }

    public void NotifyChanged()
    {
        OnContainerChanged?.Invoke();

        var quickSlot = UIManager.Instance.quickSlotController;

        for (int i = 0; i < slots.Length; i++)
        {
            quickSlot.TryRebindSlotByItemName(i);
        }
    }

}
