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

    public override void Execute(GameObject caster, GameObject target)
    {
        // 타격 코루틴 실행
        caster.GetComponent<MonoBehaviour>().StartCoroutine(HitTwiceCoroutine(caster));
    }

    private IEnumerator HitTwiceCoroutine(GameObject caster)
    {
        HitEnemies(caster);
        yield return new WaitForSeconds(delayBetweenHits);
        HitEnemies(caster);
    }

    private void HitEnemies(GameObject caster)
    {
        DebugDrawCircle(caster.transform.position, range, Color.red);

        var center = caster.transform.position;
        var hits = Physics2D.OverlapCircleAll(center, range, targetLayer);

        foreach (var hit in hits)
        {
            Debug.Log($"Hit {hit.name}");
            CameraShake.Instance.Shake(0.05f, 0.1f);
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
