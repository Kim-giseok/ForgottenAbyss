using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private void Start()
    {
        stats[StatType.HP] = 100f;
        stats[StatType.ATK] = 10f;
        Debug.Log($"HP: {stats[StatType.HP]}");
        Debug.Log($"°ø°Ý·Â: {stats[StatType.ATK]}");
    }
}
