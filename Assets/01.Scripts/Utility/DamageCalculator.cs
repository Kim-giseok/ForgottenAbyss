using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(SkillCastData castData)
    {
        return (castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier;
    }

    public static float CalculateBasicDamage(BasicAttackData baData)
    {
        return (baData.baseAttack + baData.weaponAttack) * baData.comboMultiplier;
    }
}