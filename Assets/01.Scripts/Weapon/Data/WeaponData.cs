using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Bow
}

[System.Serializable]
public class WeaponData
{
    public int Id;
    public string Name;
    public string Description;
    public int Damage;
    public float Range;
    public int ComboAttack;
    public int Skill1Id;
    public int Skill2Id;
    public WeaponType Type;
}
