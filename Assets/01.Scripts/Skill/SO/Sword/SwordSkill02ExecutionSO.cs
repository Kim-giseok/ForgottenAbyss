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

    public override void Execute(GameObject caster, GameObject target)
    {
        float facingDir = caster.transform.eulerAngles.y == 180f ? -1f : 1f; // 방향보정
        Vector3 dashDir = Vector3.right * facingDir;

        Vector3 startPos = caster.transform.position;

        Vector3 rayStartPos = startPos + dashDir * 0.1f;
        float rayLength = dashDistance - 0.1f;

        float actualDistance = dashDistance;
        float minDashDistance = dashDistance;

        RaycastHit2D hit = Physics2D.Raycast(rayStartPos, dashDir, rayLength, obstacleLayer);

        if (hit.collider != null)
        {
            actualDistance = hit.distance - 0.1f;
            actualDistance = Mathf.Max(0f, actualDistance);
            minDashDistance = actualDistance;
        }

        Vector3 targetPos = startPos + dashDir * actualDistance;

        Collider2D casterCollider = caster.GetComponent<Collider2D>();
        if (casterCollider != null)
            casterCollider.enabled = false;

        // DOTween으로 돌진
        //caster.transform.DOMove(targetPos, dashDuration)
        //    .SetEase(Ease.OutQuad)
        //    .OnComplete(() =>
        //    {
        //        if (casterCollider != null)
        //            casterCollider.enabled = true;

        //        caster.GetComponent<MonoBehaviour>().StartCoroutine(DelayedHitCoroutine(startPos, targetPos));
        //    });
    }

    private IEnumerator DelayedHitCoroutine(Vector3 start, Vector3 end)
    {
        yield return new WaitForSeconds(damageDelay);

        Vector2 center = (start + end) / 2f;
        Vector2 size = new Vector2((end - start).magnitude, hitRadius * 2f);
        float angle = Vector2.SignedAngle(Vector2.right, end - start);

        var hits = Physics2D.OverlapBoxAll(center, size, angle, targetLayer);

        foreach (var hit in hits)
        {
            Debug.Log($"Hit {hit.name}");
            CameraShake.Instance.Shake(0.05f, 0.1f);
        }

        DebugDrawBox(center, size, angle, Color.red, 0.5f);
    }

    private void DebugDrawBox(Vector2 center, Vector2 size, float angle, Color color, float duration)
    {
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        Vector2 halfSize = size / 2f;

        Vector2[] corners = new Vector2[4];
        corners[0] = center + (Vector2)(rot * new Vector3(-halfSize.x, -halfSize.y));
        corners[1] = center + (Vector2)(rot * new Vector3(-halfSize.x, halfSize.y));
        corners[2] = center + (Vector2)(rot * new Vector3(halfSize.x, halfSize.y));
        corners[3] = center + (Vector2)(rot * new Vector3(halfSize.x, -halfSize.y));

        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[1], corners[2], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);
        Debug.DrawLine(corners[3], corners[0], color, duration);
    }
}
