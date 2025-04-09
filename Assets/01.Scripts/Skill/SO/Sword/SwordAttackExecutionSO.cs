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
                CameraShake.Instance.Shake(0.05f, 0.1f);
                DealDamageToTarget(hit.gameObject, castData);
            }
        }

        DebugDrawCircle(center, range, Color.red);
    }

    private void DebugDrawCircle(Vector3 center, float radius, Color color, float duration = 0.5f)
    {
        int segments = 30;
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }
}