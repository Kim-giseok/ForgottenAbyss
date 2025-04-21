using UnityEngine;

public enum ArmorSlot { Helmet, Chest, Gloves, Boots }

[CreateAssetMenu(fileName = "NewArmorData", menuName = "SO/Equipment/ArmorData")]
public class ArmorSO : ScriptableObject
{
    public int armorId;
    public string armorName;
    public ArmorSlot slot;
    public StatType statType;
    public float value;
}