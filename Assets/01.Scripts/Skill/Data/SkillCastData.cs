using UnityEngine;

public class SkillCastData
{
    public GameObject caster;
    public GameObject target;

    public float baseAttack;
    public float weaponAttack;
    public float skillMultiplier;

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


        if (status != null)
        {
            result.baseAttack = status.stats[StatType.ATK];
        }
            

        result.weaponAttack = WeaponManager.Instance.GetCurrentWeaponAttack();
        result.skillMultiplier = data.DamageMultiplier;

        return result;
    }

    public float CalculateDamage()
    {
        return DamageCalculator.CalculateDamage(this);
    }
}
