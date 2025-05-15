using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // 기본 스탯 (직렬화를 위해 Dictionary 대신 List 사용)
    public List<StatData> stats = new List<StatData>();
    public int availableStatPoints;
    public List<InvestedStatData> investedStats = new List<InvestedStatData>();

    public List<StatBonusData> equipmentBonuses = new List<StatBonusData>();
    public List<StatMultiplierData> setBonusMultipliers = new List<StatMultiplierData>();
    public List<int> equippedArmorIDs = new List<int>();

    [System.Serializable]
    public class StatData
    {
        public int statType; // StatType enum의 int 값
        public float value;
    }

    [System.Serializable]
    public class InvestedStatData
    {
        public int statType; // StatType enum의 int 값
        public int points;
    }

    [System.Serializable]
    public class StatBonusData
    {
        public int statType;
        public int itemID;
        public float bonusValue;
    }

    [System.Serializable]
    public class StatMultiplierData
    {
        public int statType;
        public float multiplier;
    }
}
