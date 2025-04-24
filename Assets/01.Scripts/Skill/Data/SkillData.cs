using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
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
    public int MpCost;
    public float DamageMultiplier;
    public string VisualSOName;
    public string ExecutionSOName;
    public SkillType Type;

    public void CopyFrom(SkillData other)
    {
        this.Id = other.Id;
        this.Name = other.Name;
        this.Description = other.Description;
        this.CoolTime = other.CoolTime;
        this.MpCost = other.MpCost;
        this.DamageMultiplier = other.DamageMultiplier;
        this.VisualSOName = other.VisualSOName;
        this.ExecutionSOName = other.ExecutionSOName;
        this.Type = other.Type;
    }

    // 클론 메서드
    public SkillData Clone()
    {
        SkillData clonedData = new SkillData();
        clonedData.CopyFrom(this);
        return clonedData;
    }
}
