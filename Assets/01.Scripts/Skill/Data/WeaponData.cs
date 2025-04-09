using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Staff,
    Gun
}

[System.Serializable]
public class WeaponData
{
    public int Id;
    public string Name;
    public string Description;
    public int Damage;
    public float Range;
    public int basicAttackID;
    public int Skill1Id;
    public int Skill2Id;
    public WeaponType Type;
}
