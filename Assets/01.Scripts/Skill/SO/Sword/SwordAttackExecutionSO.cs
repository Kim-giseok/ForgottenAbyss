using UnityEngine;

[CreateAssetMenu(fileName = "Sword_Attack_Execution", menuName = "SO/Skill/Execution/SwordAttack")]
public class SwordAttackExecutionSO : SkillExecutionSO
{
    public float range = 2f;
    public float angleLimit = 30f;
    public LayerMask targetLayer;

    public override void Execute(GameObject caster, GameObject target)
    {
        DebugDrawCircle(caster.transform.position, range, Color.red);

        // 공격 판정
        Vector2 center = caster.transform.position;
        Vector2 forward = caster.transform.right; // 또는 바라보는 방향

        Collider2D[] hits = Physics2D.OverlapCircleAll(center, range, targetLayer);

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - center).normalized;
            float angle = Vector2.Angle(forward, toTarget);

            if (angle <= angleLimit)
            {
                Debug.Log($"Hit (in cone): {hit.name}");
                CameraShake.Instance.Shake(0.05f, 0.1f);
                //DamageCalculator.CalculateDamage(플레이어 공격력, 무기 공격력, 스킬 계수);
            }
        }
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