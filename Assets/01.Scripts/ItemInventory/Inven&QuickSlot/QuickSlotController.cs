using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    public static QuickSlotController Instance {  get; private set; }

    [SerializeField] private QuickSlot[] quickSlots; // Äü½½·Ô ½½·Ôµé
    public int SelectedIndex => selectedIndex;

    private int selectedIndex = -1; // ¼±ÅÃµÈ ½½·Ô ¾øÀ½

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetIndex(i); // ½½·Ô¸¶´Ù ÀÎµ¦½º Ãß°¡
        }
    }

    void Update()
    {
        // ½½·Ô º¯°æ
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
            quickSlots[selectedIndex].UseItem();
        }
        else
        {
            SelectSlot(index);
        }
    }

    public void SelectSlotFromOutside(int index)
    {
        SelectSlot(index);
    }

    // ½½·Ô ¼±ÅÃ
    void SelectSlot(int index)
    {
        selectedIndex = index;
        
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetSelected(i == selectedIndex);
        }
    }
}
