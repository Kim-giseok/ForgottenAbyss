using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorIconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ArmorSlot armorSlot;

    public EquippedItemUI equippedItemUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ArmorSO armor = SystemManager.Instance.equipmentManager.GetEquippedArmor(armorSlot);
        if (armor != null)
        {
            equippedItemUI.ShowItemDetails(armor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        equippedItemUI.HideItemDetails();
    }
}
