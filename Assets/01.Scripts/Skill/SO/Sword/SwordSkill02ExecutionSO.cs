using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

[CreateAssetMenu(fileName = "Sword_Skill02_Execution", menuName = "SO/Skill/Execution/SwordSkill02")]
public class SwordSkill02ExecutionSO : SkillExecutionSO
{
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public string effectKey = "SwordSkill02";
    public float damageDelay = 0.3f;
    public float hitRadius = 1f;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        SkillCastData castData = PrepareCastData(caster, target, data);

        float facingDir = caster.transform.eulerAngles.y == 180f ? -1f : 1f; // 방향보정
        Vector3 dashDir = Vector3.right * facingDir;

        Vector3 startPos = caster.transform.position;

        Vector3 rayStartPos = startPos + dashDir * 0.1f;
        float rayLength = dashDistance - 0.1f;

        float actualDistance = dashDistance;
        RaycastHit2D hit = Physics2D.Raycast(rayStartPos, dashDir, rayLength, obstacleLayer);

        if (hit.collider != null)
        {
            actualDistance = Mathf.Max(0f, hit.distance - 0.1f);
        }

        Vector3 targetPos = startPos + dashDir * actualDistance;

        Collider2D casterCollider = caster.GetComponent<Collider2D>();
        if (casterCollider != null)
            casterCollider.enabled = false;

        SystemManager.Instance.actionBufferUtil.StartCoroutine(DashCoroutine(caster, startPos, targetPos, casterCollider, () =>
        {
            SystemManager.Instance.actionBufferUtil.StartCoroutine(DelayedHitCoroutine(startPos, targetPos, castData));
        }));
    }

    private IEnumerator DashCoroutine(GameObject caster, Vector3 startPos, Vector3 targetPos, Collider2D casterCollider, System.Action onComplete)
    {
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dashDuration);
            float easeT = 1f - Mathf.Pow(1f - t, 2f); // Ease.OutQuad

            caster.transform.position = Vector3.Lerp(startPos, targetPos, easeT);
            yield return null;
        }

        caster.transform.position = targetPos;

        if (casterCollider != null)
            casterCollider.enabled = true;

        onComplete?.Invoke();
    }

    private IEnumerator DelayedHitCoroutine(Vector3 start, Vector3 end, SkillCastData castData)
    {
        yield return new WaitForSeconds(damageDelay);

        PlaySound("AWP_Dagger_UnSheath");

        float extraLength = 3f;
        Vector2 dashDir = (end - start).normalized;

        Vector2 extendedStart = (Vector2)start - dashDir * (extraLength / 2f);
        Vector2 extendedEnd = (Vector2)end + dashDir * ((extraLength / 2f) - 1f);

        Vector2 center = (extendedStart + extendedEnd) / 2f + Vector2.up * 0.5f;
        Vector2 size = new Vector2((extendedEnd - extendedStart).magnitude, hitRadius * 2f);
        float angle = Vector2.SignedAngle(Vector2.right, extendedEnd - extendedStart);

        var hits = Physics2D.OverlapBoxAll(center, size, angle, targetLayer);

        foreach (var hit in hits)
        {
            DealDamageToTarget(hit.gameObject, castData);
            Debug.Log($"Hit {hit.name}");
            
        }
  
        GameManager.Instance.cameraShake.Shake(0.2f, 0.3f);
        DebugDrawUtil.DrawBox(center, size, angle, Color.red, 0.5f);
    }

    public override SkillExecutionSO Clone()
    {
        SwordSkill02ExecutionSO clone = CreateInstance<SwordSkill02ExecutionSO>();

        clone.dashDistance = this.dashDistance;
        clone.dashDuration = this.dashDuration;
        clone.effectKey = this.effectKey;
        clone.damageDelay = this.damageDelay;
        clone.hitRadius = this.hitRadius;
        clone.targetLayer = this.targetLayer;
        clone.obstacleLayer = this.obstacleLayer;

        return clone;
    }
}
