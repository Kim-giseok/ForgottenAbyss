using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public static QuickSlotController Instance {  get; private set; }

    [SerializeField] private QuickSlot[] quickSlots; // 퀵슬롯 슬롯들
    private Item[] quickSlotItems;
    public int SelectedIndex => selectedIndex;
    private int selectedIndex = -1; // 선택된 슬롯 없음

    private void Awake()
    {
        Instance = this;
        quickSlotItems = new Item[quickSlots.Length];

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetIndex(i); // 슬롯마다 인덱스 추가
        }
    }

    void Update()
    {
        // 슬롯 변경
        if (Input.GetKeyDown(KeyCode.Alpha1)) HandleSlotInput(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) HandleSlotInput(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) HandleSlotInput(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) HandleSlotInput(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) HandleSlotInput(4);
    }

    private void HandleSlotInput(int index)
    {
        if (selectedIndex == index)
        {
            // 같은 슬롯을 다시 누르면 아이템 사용
            quickSlots[index].UseItem();
        }
        else
        {
            SelectSlot(index); // 다른 슬롯을 누르면 선택만 바뀜
        }
    }

    public bool IsAlreadyAssigned(Item item)
    {
        return quickSlots.Any(slot => slot.HasItem(item));
    }

    // 마우스 클릭할때 사용
    public void SelectSlotFromOutside(int index)
    {
        SelectSlot(index);
    }

    // 슬롯 선택
    void SelectSlot(int index)
    {
        selectedIndex = index;

        // 선택된 슬롯외에 나머지는 해제
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetSelected(i == selectedIndex);
        }
    }

    public void SetItemToQuickSlot(int index, Item item)
    {
        if (index < 0 || index >= quickSlotItems.Length) return;

        quickSlotItems[index] = item;
        quickSlots[index].SetItem(item);
    }

    public void NotifyItemRemoved(Item removedItem)
    {
        foreach (var slot in quickSlots)
        {
            if (slot.HasItem(removedItem))
            {
                slot.MarkToClearAfterCooldown();
            }
        }
    }

    public void RefreshSlotAmount(Item item)
    {
        foreach (var slot in quickSlots)
        {
            if (slot.HasItem(item))
            {
                slot.UpdateAmount(item.currentAmount);
            }
        }
    }
}
