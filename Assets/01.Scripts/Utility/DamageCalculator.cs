using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(SkillCastData castData)
    {
        return (castData.baseAttack + castData.weaponAttack) * castData.skillMultiplier;
    }
}