using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private void Start()
    {
        stats[StatType.HP] = 100f; //초기 HP
        stats[StatType.ATK] = 10f; //초기 공격력
        stats[StatType.LEVEL] = 1f; //초기 레벨
        stats[StatType.EXP] = 0f; //초기 경험치
        Debug.Log($"HP: {stats[StatType.HP]}");
        Debug.Log($"공격력: {stats[StatType.ATK]}");
        Debug.Log($"레벨: {stats[StatType.LEVEL]}");
        Debug.Log($"경험치: {stats[StatType.EXP]}");
    }
}
