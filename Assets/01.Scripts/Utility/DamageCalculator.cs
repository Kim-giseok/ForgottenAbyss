using UnityEngine;

public struct DamageResult
{
    public float damage;
    public bool isCrit;
}

public static class DamageCalculator
{
    public static DamageResult CalculateDamage(SkillCastData castData)
    {
        // 기본 데미지
        float damage = (castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier;

        // 크리티컬 판정
        bool isCrit = Random.value < castData.critChance;
        float finalDmg = isCrit ? damage * castData.critDamageMultiplier : damage;

        Debug.Log(isCrit
            ? $"CRIT! ({damage}) * {castData.critDamageMultiplier} = {finalDmg}"
            : $"{damage}");

        return new DamageResult { damage = finalDmg, isCrit = isCrit };
    }

    public static DamageResult CalculateBasicDamage(BasicAttackData baData)
    {
        // 기본 데미지
        float damage = (baData.baseAttack + baData.weaponAttack) * baData.comboMultiplier;

        // 크리티컬 판정
        bool isCrit = Random.value < baData.critChance;
        float finalDmg = isCrit ? damage * baData.critDamageMultiplier : damage;

        Debug.Log(isCrit
            ? $"CRIT! ({damage}) * {baData.critDamageMultiplier} = {finalDmg}"
            : $"{damage}");

        return new DamageResult { damage = finalDmg, isCrit = isCrit };
    }
}