using System;

public interface IItemContainer
{
    // 아이템 추가
    bool AddItem(Item item, int amount);

    // 특정 슬롯 위치에 아이템 추가
    bool AddItemAt(int index, Item item, int amount);

    // 아이템 제거
    bool RemoveItem(Item item, int amount);

    bool RemoveItemAt(int index);

    // 두 슬롯 간 아이템 교체
    bool SwapItems(int indexA, int indexB);

    // 해당 아이템을 받을 수 있는지 확인
    bool CanAccept(Item item);

    // 슬롯 정보 조회
    ISlot GetSlot(int index);

    // 슬롯 개수
    int SlotCount { get; }

    // 이벤트 기반으로 갱신 감지
    event Action OnContainerChanged;
}
