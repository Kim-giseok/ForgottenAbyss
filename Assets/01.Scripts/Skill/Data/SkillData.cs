using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    BasicAttack,
    Active1,
    Active2,
    Memory
}

[System.Serializable]
public class SkillData
{
    public int Id;
    public string Name;
    public string Description;
    public float CoolTime;
    public float DamageMultiplier;
    public string VisualSOName;
    public string ExecutionSOName;
    public SkillType Type;
}
