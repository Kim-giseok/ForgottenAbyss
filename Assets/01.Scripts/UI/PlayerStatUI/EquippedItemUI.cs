using UnityEngine;
using UnityEngine.UI;

public class EquippedItemUI : MonoBehaviour
{
    public Image helmetIcon;
    public Image armorIcon;
    public Image glovesIcon;
    public Image shoesIcon;

    private EquipmentManager equipManager => SystemManager.Instance.equipmentManager;

    private void OnEnable()
    {
        equipManager.OnEquipArmor += OnArmorChanged;
        equipManager.OnUnequipArmor += OnArmorChanged;
        UpdateUI();
    }

    private void OnDisable()
    {
        equipManager.OnEquipArmor -= OnArmorChanged;
        equipManager.OnUnequipArmor -= OnArmorChanged;
    }

    private void OnArmorChanged(ArmorSO armor)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        SetIcon(helmetIcon, equipManager.GetEquippedArmor(ArmorSlot.Helmet));
        SetIcon(armorIcon, equipManager.GetEquippedArmor(ArmorSlot.Chest));
        SetIcon(glovesIcon, equipManager.GetEquippedArmor(ArmorSlot.Gloves));
        SetIcon(shoesIcon, equipManager.GetEquippedArmor(ArmorSlot.Boots));
    }

    private void SetIcon(Image img, ArmorSO armor)
    {
        if (armor != null)
        {
            img.sprite = armor.itemIcon;
            img.enabled = true;
        }
        else
        {
            img.sprite = null;
            img.enabled = false;
        }
    }
}
