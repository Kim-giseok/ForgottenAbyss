using UnityEngine;

public static class DamageCalculator
{
    public static float CalculateDamage(float baseAttack, float weaponAttack, float skillMultiplier)
    {
        return (baseAttack + weaponAttack) * skillMultiplier;
    }
}