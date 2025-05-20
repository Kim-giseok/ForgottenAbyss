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
        // 데미지 공식 : (플레이어 현재 공격력 * 무기의 계수) * 해당 스킬의 계수
        float damage = (castData.baseAttack * castData.weaponAttack) * castData.skillMultiplier;

        // 크리티컬 판정 시 크리티컬 데미지 계수 적용
        bool isCrit = Random.value < castData.critChance;
        float finalDmg = isCrit ? damage * castData.critDamageMultiplier : damage;

        Debug.Log(isCrit
            ? $"CRIT! ({damage}) * {castData.critDamageMultiplier} = {finalDmg}"
            : $"{damage}");

        return new DamageResult { damage = finalDmg, isCrit = isCrit };
    }

    public static DamageResult CalculateBasicDamage(BasicAttackData baData)
    {
        //데미지 공식 : (플레이어 현재 공격력 *무기의 계수) * 해당 콤보의 계수
        float damage = (baData.baseAttack * baData.weaponAttack) * baData.comboMultiplier;

        // 크리티컬 판정 시 크리티컬 데미지 계수 적용
        bool isCrit = Random.value < baData.critChance;
        float finalDmg = isCrit ? damage * baData.critDamageMultiplier : damage;

        Debug.Log(isCrit
            ? $"CRIT! ({damage}) * {baData.critDamageMultiplier} = {finalDmg}"
            : $"{damage}");

        return new DamageResult { damage = finalDmg, isCrit = isCrit };
    }
}