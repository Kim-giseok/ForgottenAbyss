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

    public Dictionary<StatType, float> baseValues = new Dictionary<StatType, float>();

    public static readonly Dictionary<string, ArmorSetBonus> SetBonuses = new Dictionary<string, ArmorSetBonus>
    {
        { "무쇠", new ArmorSetBonus("무쇠", "최대 체력 +5%, 방어력 +5%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.05f),
                (StatType.DEF, 0.05f)
            })
        },
        { "청동", new ArmorSetBonus("청동", "최대 체력 +10%, 방어력 +10%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.1f),
                (StatType.DEF, 0.1f)
            })
        },
        { "황금", new ArmorSetBonus("황금", "최대 체력 +20%, 방어력 +20%, 공격력 +10%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.2f),
                (StatType.DEF, 0.2f),
                (StatType.ATK, 0.1f)
            })
        },
        //{ "Ruby", new ArmorSetBonus("Ruby", "최대 체력 +10%, 방어력 +10%, 공격력 +20%, 크리티컬 확률 +10%", new List<(StatType, float)>
        //    {
        //        (StatType.MaxHP, 0.1f),
        //        (StatType.DEF, 0.1f),
        //        (StatType.ATK, 0.2f),
        //        (StatType.CRITICAL, 10f)
        //    })
        //},
        { "다이아몬드", new ArmorSetBonus("다이아몬드", "최대 체력 +30%, 방어력 +30%, 공격력 +20%", new List<(StatType, float)>
            {
                (StatType.MaxHP, 0.3f),
                (StatType.DEF, 0.3f),
                (StatType.ATK, 0.2f)            
                
            })
        },
        { "광전사", new ArmorSetBonus("광전사", "최대 체력 -30%, 방어력 -30%, 공격력 +40%", new List<(StatType, float)>
            {
                (StatType.MaxHP, -0.3f),
                (StatType.DEF, -0.3f),
                (StatType.ATK, 0.4f)
            })
        },
        { "암살자", new ArmorSetBonus("암살자", "크리티컬 확률 +30%, 크리티컬 데미지 30%", new List<(StatType, float)>
            {
                (StatType.CRITICAL, 30f),
                (StatType.CRITICAL_DAMAGE, 30f),
            })
        }
    };
}
