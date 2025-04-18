using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(SkillCastData castData)
    {
        Debug.Log($"{castData.baseAttack} + {castData.weaponAttack} * {castData.skillMultiplier} = " +
            $"{(castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier}");
        return (castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier;
    }

    public static float CalculateBasicDamage(BasicAttackData baData)
    {
        return (baData.baseAttack + baData.weaponAttack) * baData.comboMultiplier;
    }
}