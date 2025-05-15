using UnityEngine;

public static class SlotUtils
{
    public static bool TryUseSlot(ISlot slot)
    {
        if (slot == null || slot.IsEmpty)
            return false;

        return slot.Use(); // 내부적으로 Item.Use() 호출 + 수량 감소 + Clear()
    }
}
