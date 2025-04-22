using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackData
{
    public GameObject caster;
    public GameObject target;

    public float baseAttack;
    public float weaponAttack;
    public float comboMultiplier;

    public Vector3 position => caster.transform.position;

    private BasicAttackData() { }

    public static BasicAttackData Create(GameObject caster, GameObject target, float comboMultiplier)
    {
        var result = new BasicAttackData();
        result.caster = caster;
        result.target = target;

        var status = caster.GetComponent<PlayerStatus>();
        if (status != null)
            result.baseAttack = status.GetStat(StatType.ATK);

        result.weaponAttack = WeaponManager.Instance.GetCurrentWeaponAttack();
        result.comboMultiplier = comboMultiplier;

        return result;
    }

    public float CalculateDamage()
    {
        return DamageCalculator.CalculateBasicDamage(this);
    }
}
