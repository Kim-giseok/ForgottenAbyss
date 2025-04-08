using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    HP,
    MP,
    ATK,
    DEF,
    LEVEL,
    EXP,
    GOLD
}
public class CharacterStatus : MonoBehaviour
{
    public Dictionary<StatType, float> stats = new Dictionary<StatType, float>();

}
