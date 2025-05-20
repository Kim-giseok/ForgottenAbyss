using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword_Skill01_Execution", menuName = "SO/Skill/Execution/SwordSkill01")]
public class SwordSkill01ExecutionSO : SkillExecutionSO
{
    public float range = 1.5f;
    public LayerMask targetLayer;
    public float delayBetweenHits = 0.1f;
    public string effectKey = "SwordSkill01";

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        var castData = PrepareCastData(caster, target, data);

        SystemManager.Instance.actionBufferUtil.StartCoroutine(HitTwiceCoroutine(caster, castData));
    }

    private IEnumerator HitTwiceCoroutine(GameObject caster, SkillCastData castData)
    {
        PlaySound("SwordAttack4");
        HitEnemies(caster, castData);

        yield return new WaitForSeconds(delayBetweenHits);

        PlaySound("SwordAttack4");
        HitEnemies(caster, castData);
    }

    private void HitEnemies(GameObject caster, SkillCastData castData)
    {
        Vector2 center = caster.transform.position;
        Collider2D[] hits = GetEnemiesInRange(center, range, targetLayer);

        foreach (var hit in hits)
        {
            Debug.Log($"Hit {hit.name}");
            GameManager.Instance.cameraShake.Shake(0.2f, 0.3f);
            DealDamageToTarget(hit.gameObject, castData);
            KnockbackUtil.ApplyKnockback(hit.gameObject, caster.transform.position, 4f);
        }

        DebugDrawUtil.DrawCircle(center, range, Color.red);
    }

    public override SkillExecutionSO Clone()
    {
        SwordSkill01ExecutionSO clone = CreateInstance<SwordSkill01ExecutionSO>();

        clone.range = this.range;
        clone.targetLayer = this.targetLayer;
        clone.delayBetweenHits = this.delayBetweenHits;
        clone.effectKey = this.effectKey;

        return clone;
    }
}
