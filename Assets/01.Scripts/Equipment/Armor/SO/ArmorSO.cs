using UnityEngine;
using System.Collections.Generic;

public enum ArmorSlot { Helmet, Chest, Gloves, Boots }
public enum BonusType {  Flat, Percent }

[System.Serializable]
public class ArmorStatBonus
{
    public StatType statType;
    public float value;
    public BonusType bonusType;
}

[CreateAssetMenu(fileName = "NewArmorData", menuName = "SO/Equipment/ArmorData")]
public class ArmorSO : Item
{
    public int armorId;
    public ArmorSlot slot;

    public string setName;

    public List<ArmorStatBonus> statBonuses = new();
}