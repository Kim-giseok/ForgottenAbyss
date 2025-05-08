using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSetBonus
{
    public string SetName;
    public List<(StatType stat, float multiplier)> Bonuses;
    public string Description;

    public ArmorSetBonus(string setName, string description, List<(StatType, float)> bonuses)
    {
        SetName = setName;
        Description = description;
        Bonuses = bonuses;
    }

    public static readonly Dictionary<string, ArmorSetBonus> SetBonuses = new Dictionary<string, ArmorSetBonus>
    {
        { "Iron", new ArmorSetBonus("Iron", "최대 체력 +5%, 방어력 +5%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.05f),
                (StatType.DEF, 0.05f)
            })
        },
        { "Bronze", new ArmorSetBonus("Bronze", "최대 체력 +10%, 방어력 +10%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.1f),
                (StatType.DEF, 0.1f)
            })
        },
        { "Gold", new ArmorSetBonus("Gold", "최대 체력 +10%, 공격력 +10%, 크리티컬 확률 +5%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.1f),
                (StatType.ATK, 0.1f),
                (StatType.CRITICAL, 0.05f)
            })
        },
        { "Ruby", new ArmorSetBonus("Ruby", "최대 체력 +10%, 방어력 +10%, 공격력 +20%, 크리티컬 확률 +10%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.1f),
                (StatType.DEF, 0.1f),
                (StatType.ATK, 0.2f),
                (StatType.CRITICAL, 0.1f)
            })
        },
        { "Diamond", new ArmorSetBonus("Diamond", "모든 스탯 +20%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.2f),
                (StatType.MaxMP, 0.2f),
                (StatType.DEF, 0.2f),
                (StatType.SPEED, 0.2f),
                (StatType.ATK, 0.2f),             
                (StatType.CRITICAL, 0.2f),
                (StatType.CRITICAL_DAMAGE, 0.2f),
                (StatType.COOLDOWN_REDUCTION, 0.2f)
            })
        }
    };
}
