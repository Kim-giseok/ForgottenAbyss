using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public Dictionary<StatType, float> baseStats = new Dictionary<StatType, float>();
    public Dictionary<StatType, float> equipmentBonuses = new Dictionary<StatType, float>();
    public Dictionary<StatType, float> setBonusMultipliers = new Dictionary<StatType, float>();
    public HashSet<int> equippedArmorIDs = new HashSet<int>();

    public delegate void StatChangedHandler(StatType type, float newValue); // 스탯이 변경되면 발생하는 이벤트
    public event StatChangedHandler OnStatChanged;

    public virtual void SetStat(StatType type, float value)
    {
        stats[type] = value; // 값 설정
        SetHP(type);
        OnStatChanged?.Invoke(type, value); // 스탯 변경 이벤트 발생
    }

    public float GetCurrentHP()
    {
        return stats[StatType.CurrentHP];
    }

    public void SetHP(StatType type)
    {
        if (type == StatType.MaxHP && stats.ContainsKey(StatType.CurrentHP) &&
        stats[StatType.CurrentHP] > stats[StatType.MaxHP])
        {
            stats[StatType.CurrentHP] = stats[StatType.MaxHP]; //최대 체력으로 맞춤
        }
    }

    public void ApplyEquipmentBonus(StatType type, float bonusValue, int itemID)
    {
        if (!stats.ContainsKey(type)) return;

        if (!equipmentBonuses.ContainsKey(type))
            equipmentBonuses[type] = 0f;

        if (!equippedArmorIDs.Contains(itemID))
        {
            equippedArmorIDs.Add(itemID);
            equipmentBonuses[type] += bonusValue;
        }
        
        stats[type] += bonusValue; // 장비 보너스 적용

        SetHP(type);
        OnStatChanged?.Invoke(type, stats[type]);
    }

    public void RemoveEquipmentBonus(StatType type, float bonusValue)
    {
        if (!stats.ContainsKey(type)) return;

        if (equipmentBonuses.ContainsKey(type))
        {
            equipmentBonuses[type] -= bonusValue; // 보너스 값 제거
            if (equipmentBonuses[type] <= 0) equipmentBonuses[type] = 0f; // 0 이하로 내려가면 초기화
        }

        stats[type] -= bonusValue; // 장비 보너스 제거
        SetHP(type);
        OnStatChanged?.Invoke(type, stats[type]);
    }

    public void ApplySetBonus(StatType type, float multiplier)
    {
        if (!stats.ContainsKey(type)) return;

        setBonusMultipliers[type] = 1 + multiplier;

        if (type == StatType.CRITICAL || type == StatType.CRITICAL_DAMAGE || type == StatType.COOLDOWN_REDUCTION)
        {
            stats[type] = stats[type] + multiplier;
        }
        else
        {
            stats[type] = stats[type] * setBonusMultipliers[type];
        }

        SetHP(type);
        OnStatChanged?.Invoke(type, stats[type]);
    }

    public void RemoveSetBonus(StatType type, float multiplier)
    {
        if (!stats.ContainsKey(type) || !setBonusMultipliers.ContainsKey(type)) return;

        if (type == StatType.CRITICAL || type == StatType.CRITICAL_DAMAGE || type == StatType.COOLDOWN_REDUCTION)
        {
            stats[type] = stats[type] - multiplier;
        }
        else
        {
            stats[type] = stats[type] / setBonusMultipliers[type];
        }

        setBonusMultipliers.Remove(type);

        SetHP(type);
        OnStatChanged?.Invoke(type, stats[type]);
    }

    public float GetEquipmentBonus(StatType type)
    {
        if (!equipmentBonuses.ContainsKey(type)) return 0f;

        float bonus = equipmentBonuses[type];

        Debug.Log($"[DEBUG] {type} - 장비 보너스 저장된 값: {bonus}");

        return bonus;
    }

    public float GetSetBonus(StatType type)
    {
        return setBonusMultipliers.ContainsKey(type) ? setBonusMultipliers[type] : 1.0f; // 저장된 세트 옵션 보너스 반환
    }

    public float GetBaseStat(StatType type)
    {

        if (!stats.ContainsKey(type)) return 0f;

        float totalValue = stats[type];

        float setBonusMultiplier = setBonusMultipliers.ContainsKey(type) ? setBonusMultipliers[type] : 1.0f;
        float equipmentBonus = GetEquipmentBonus(type);
        float baseValue = (totalValue - equipmentBonus) / setBonusMultiplier;

        Debug.Log($"[DEBUG] {type} - Total: {totalValue}, Set Multiplier: {setBonusMultiplier}, Equip Bonus: {equipmentBonus}, Base: {baseValue}");

        return baseValue;
    }

    public void ResetStats()
    {
        float currentHP = stats[StatType.CurrentHP];
        float currentMP = stats[StatType.CurrentMP];
        float exp = stats[StatType.EXP];
        float maxExp = stats[StatType.MaxEXP];

        stats = new Dictionary<StatType, float>(baseStats);
        equipmentBonuses.Clear();
        setBonusMultipliers.Clear();
        equippedArmorIDs.Clear();

        stats[StatType.CurrentHP] = currentHP;
        stats[StatType.CurrentMP] = currentMP;
        stats[StatType.EXP] = exp;
        stats[StatType.MaxEXP] = maxExp;

        Debug.Log("[ResetStats] 플레이어 스탯 초기화 완료");
    }
}
