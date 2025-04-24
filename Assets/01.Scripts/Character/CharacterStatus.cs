using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    CurrentHP,
    MaxHP,
    CurrentMP,
    MaxMP,
    ATK,
    DEF,
    LEVEL,
    EXP,
    MaxEXP,
    GOLD,
    SPEED,
    CRITICAL,
    CRITICAL_DAMAGE,
    COOLDOWN_REDUCTION
}
public class CharacterStatus : MonoBehaviour
{
    public Dictionary<StatType, float> stats = new Dictionary<StatType, float>();

    public delegate void StatChangedHandler(StatType type, float newValue); // 스탯이 변경되면 발생하는 이벤트
    public event StatChangedHandler OnStatChanged;

    public virtual void SetStat(StatType type, float value)
    {
        stats[type] = value; // 값 설정
        OnStatChanged?.Invoke(type, value); // 스탯 변경 이벤트 발생
    }
}
