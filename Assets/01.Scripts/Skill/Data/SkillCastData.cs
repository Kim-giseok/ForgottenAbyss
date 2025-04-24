using UnityEngine;

public class SkillCastData
{
    public GameObject caster;
    public GameObject target;

    public float baseAttack;
    public float weaponAttack;
    public float skillMultiplier;
    public float critChance;
    public float critDamageMultiplier;

    public Vector3 position => caster.transform.position;

    private SkillCastData() { }

    public static SkillCastData Create(GameObject caster, GameObject target, SkillData data)
    {
        var result = new SkillCastData();
        result.caster = caster;
        result.target = target;

        var status = caster.GetComponent<PlayerStatus>();
        if(status == null)
        {
            Debug.Log("status 가 널입니다.");
        }
        else
        {
            result.baseAttack = status.GetStat(StatType.ATK);
            result.critChance = status.GetStat(StatType.CRITICAL) / 100f;
            result.critDamageMultiplier = status.GetStat(StatType.CRITICAL_DAMAGE) / 100f;  // << 추후 크리티컬 적용시
        }
            

        result.weaponAttack = WeaponManager.Instance.GetCurrentWeaponAttack();
        result.skillMultiplier = data.DamageMultiplier;

        return result;
    }

    public DamageResult CalculateDamage()
    {
        return DamageCalculator.CalculateDamage(this);
    }
}
