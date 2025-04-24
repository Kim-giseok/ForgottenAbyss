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
    public float critChance;
    public float critDamageMultiplier;

    public Vector3 position => caster.transform.position;

    private BasicAttackData() { }

    public static BasicAttackData Create(GameObject caster, GameObject target, float comboMultiplier)
    {
        var result = new BasicAttackData();
        result.caster = caster;
        result.target = target;

        var status = caster.GetComponent<PlayerStatus>();

        if (status == null)
        {
            Debug.Log("status 가 널입니다.");
        }
        else
        {
            result.baseAttack = status.GetStat(StatType.ATK);
            result.critChance = status.GetStat(StatType.CRITICAL);
            result.critDamageMultiplier = status.GetStat(StatType.CRITICAL_DAMAGE);  // < 추후 크리티컬 적용시
        }

        result.weaponAttack = WeaponManager.Instance.GetCurrentWeaponAttack();
        result.comboMultiplier = comboMultiplier;

        return result;
    }

    public float CalculateDamage()
    {
        return DamageCalculator.CalculateBasicDamage(this);
    }
}
