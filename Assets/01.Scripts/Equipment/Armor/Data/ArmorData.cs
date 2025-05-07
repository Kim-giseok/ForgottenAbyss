using System.Collections.Generic;

[System.Serializable]
public class ArmorStatBonusData
{
    public string StatType;
    public float Value;
    public string BonusType;
}

[System.Serializable]
public class ArmorData
{
    public int Id;
    public string Name;
    public string Slot;
    public string SetName;
    public List<ArmorStatBonusData> StatBonuses;
}