using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow_Skill01_Execution", menuName = "SO/Skill/Execution/BowSkill01")]
public class BowSkill01ExecutionSO : SkillExecutionSO
{
    float range = 5f;
    public LayerMask hitMask;
    public float damageDelay;
    public float hitRadius;

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        SkillCastData castData = PrepareCastData(caster, target, data);

        Vector2 origin = (Vector2)caster.transform.position + new Vector2(2f, 0f);
        Vector2 direction = caster.transform.right;

        CoroutinRunner.Instance.RunCoroutine(ExecuteWithEffectDelay(caster, origin, direction, castData));
        //// 관통 판정 (RaycastAll)
        //RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, range, hitMask);

        //foreach (var hit in hits)
        //{
        //    DealDamageToTarget(hit.collider.gameObject, castData);
        //}
    }

    private IEnumerator ExecuteWithEffectDelay(GameObject caster, Vector2 origin, Vector2 direction, SkillCastData castData)
    {
        yield return new WaitForSeconds(damageDelay);

        float extraLength = 4f; // 범위 확장값
        Vector2 dashDir = direction.normalized;
        Vector2 extendedStart = (Vector2)origin - dashDir * (extraLength / 2f);
        Vector2 extendedEnd = (Vector2)origin + dashDir * (range + extraLength / 2f);  // 기본 범위 + 확장

        Vector2 center = (extendedStart + extendedEnd) / 2f;
        Vector2 size = new Vector2((extendedEnd - extendedStart).magnitude, hitRadius * 2f);  // hitRadius는 필요에 맞게 설정
        float angle = Vector2.SignedAngle(Vector2.right, extendedEnd - extendedStart);

        // OverlapBox로 판정
        var hits = Physics2D.OverlapBoxAll(center, size, angle, hitMask);

        foreach (var hit in hits)
        {
            DealDamageToTarget(hit.gameObject, castData);  // 데미지 처리
            Debug.Log($"Hit {hit.name}");
            CameraShake.Instance.Shake(0.1f, 0.2f);  // 카메라 쉐이크
        }

        DebugDrawUtil.DrawBox(center, size, angle, Color.red, 0.5f);
    }
}
