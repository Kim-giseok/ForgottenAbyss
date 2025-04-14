using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private QuickSlot[] quickSlots; // ƒ¸ΩΩ∑‘ ΩΩ∑‘µÈ
    private int selectedIndex = -1; // º±≈√µ» ΩΩ∑‘ æ¯¿Ω


    void Update()
    {
        // ΩΩ∑‘ ∫Ø∞Ê
        if (Input.GetKeyDown(KeyCode.Alpha1)) HandleSlotInput(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) HandleSlotInput(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) HandleSlotInput(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) HandleSlotInput(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) HandleSlotInput(4);

        // æ∆¿Ã≈€ ªÁøÎ
        if (Input.GetKeyDown(KeyCode.U) && selectedIndex >= 0)
        {
            quickSlots[selectedIndex].UseItem();
        }
    }

    void HandleSlotInput(int index)
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

    // ΩΩ∑‘ º±≈√
    void SelectSlot(int index)
    {
        selectedIndex = index;
        
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].SetSelected(i == selectedIndex);
        }
    }
}
