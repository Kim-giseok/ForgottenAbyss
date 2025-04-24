using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillExecutionSO", menuName = "SO/Skill/Execution")]
public abstract class SkillExecutionSO : ScriptableObject
{
    [TextArea]
    public string description;

    // 스킬 실행
    public abstract void Execute(GameObject caster, GameObject target, SkillData data);

    public void ExecuteSkill(GameObject caster, GameObject target, SkillData skillData)
    {
        SkillCastData castData = PrepareCastData(caster, target, skillData);
        DealDamageToTarget(target, castData);
    }

    protected SkillCastData PrepareCastData(GameObject caster, GameObject target, SkillData data)
    {
        return SkillCastData.Create(caster, target, data);
    }

    protected void DealDamageToTarget(GameObject target, SkillCastData castData)
    {
        var enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            var result = castData.CalculateDamage();
            Debug.Log($"damage : {result.damage}");
            enemy.GetDamage(result.damage);

            Vector3 worldPos = target.transform.position + Vector3.up * 1f;
            DamageTextManager.Instance.ShowDamage(worldPos, (int)result.damage, result.isCrit);
        }
    }

    protected Collider2D[] GetEnemiesInRange(Vector3 center, float range, LayerMask layer)
    {
        return Physics2D.OverlapCircleAll(center, range, layer);
    }
}
