using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private void Start()
    {
        stats[StatType.HP] = 100f; //�ʱ� HP
        stats[StatType.MP] = 100f; //�ʱ� MP
        stats[StatType.ATK] = 10f; //�ʱ� ���ݷ�
        stats[StatType.DEF] = 10f; //�ʱ� ����
        stats[StatType.LEVEL] = 1f; //�ʱ� ����
        stats[StatType.EXP] = 0f; //�ʱ� ����ġ
        stats[StatType.GOLD] = 0f; //�ʱ� ���

        Debug.Log($"HP: {stats[StatType.HP]}");
        Debug.Log($"MP: {stats[StatType.MP]}");
        Debug.Log($"���ݷ�: {stats[StatType.ATK]}");
        Debug.Log($"����: {stats[StatType.DEF]}");
        Debug.Log($"����: {stats[StatType.LEVEL]}");
        Debug.Log($"����ġ: {stats[StatType.EXP]}");
        Debug.Log($"���: {stats[StatType.GOLD]}");
    }
}
