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
public class ArmorSO : ScriptableObject
{
    public int armorId;
    public string armorName;
    public string description;
    public ArmorSlot slot;
    public Sprite icon;

    public List<ArmorStatBonus> statBonuses = new();
}