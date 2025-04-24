using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(SkillCastData castData)
    {
        // 기본 데미지
        float damage = (castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier;

        // 크리티컬 판정
        bool isCrit = Random.value < castData.critChance;

        if (isCrit)
        {
            damage *= castData.critDamageMultiplier;
            Debug.Log($"(({castData.baseAttack} + {castData.weaponAttack}) * {castData.skillMultiplier}) * {castData.critDamageMultiplier} = {damage}");
        }
        else
            Debug.Log($"{castData.baseAttack} + {castData.weaponAttack} * {castData.skillMultiplier} = {damage}");

        return damage;
    }

    public static float CalculateBasicDamage(BasicAttackData baData)
    {
        // 기본 데미지
        float damage = (baData.baseAttack + baData.weaponAttack) * baData.comboMultiplier;

        // 크리티컬 판정
        bool isCrit = Random.value < baData.critChance;

        if (isCrit)
        {
            damage *= baData.critDamageMultiplier;
            Debug.Log($"(({baData.baseAttack} + {baData.weaponAttack}) * {baData.comboMultiplier}) * {baData.critDamageMultiplier} = {damage}");
        }
        else
            Debug.Log($"{baData.baseAttack} + {baData.weaponAttack} * {baData.comboMultiplier} = {damage}");

        return damage;
    }
}