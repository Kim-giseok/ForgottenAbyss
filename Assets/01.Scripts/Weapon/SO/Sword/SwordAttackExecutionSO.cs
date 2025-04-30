using UnityEngine;

[CreateAssetMenu(fileName = "Sword_Attack_Execution", menuName = "SO/Skill/Execution/SwordAttack")]
public class SwordAttackExecutionSO : SkillExecutionSO
{
    public float range = 2f;
    public float angleLimit = 30f;
    public LayerMask targetLayer;

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        var castData = PrepareCastData(caster, target, data);

        Vector2 center = caster.transform.position;
        Vector2 forward = caster.transform.right;

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, range, targetLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - center).normalized;
            float angle = Vector2.Angle(forward, toTarget);

            if (angle <= angleLimit)
            {
                Debug.Log($"Hit (in cone): {hit.name}");
                //CameraShake.Instance.Shake(0.05f, 0.1f);
                DealDamageToTarget(hit.gameObject, castData);
                KnockbackUtil.ApplyKnockback(hit.gameObject, caster.transform.position, 1f);
            }
        }

        DebugDrawUtil.DrawFan(caster.transform.position, range, angleLimit * 2f, caster.transform.right, Color.red, 0.5f);
    }
}