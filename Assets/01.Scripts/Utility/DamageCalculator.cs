using UnityEngine;

public static class DamageCalculator
{
    // 여기에 크리티컬 적용
    public static float CalculateDamage(SkillCastData castData)
    {
        Debug.Log($"{castData.baseAttack} + {castData.weaponAttack} * {castData.skillMultiplier} = " +
            $"{(castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier}");

        return (castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier;
    }

    public static float CalculateBasicDamage(BasicAttackData baData)
    {
        Debug.Log($"{baData.baseAttack} + {baData.weaponAttack} * {baData.comboMultiplier} = " +
            $"{(baData.baseAttack + baData.weaponAttack) * baData.comboMultiplier}");

        return (baData.baseAttack + baData.weaponAttack) * baData.comboMultiplier;
    }
}