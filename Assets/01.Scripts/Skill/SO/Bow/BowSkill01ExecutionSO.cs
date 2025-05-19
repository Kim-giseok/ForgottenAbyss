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

        Vector2 direction = caster.transform.right.normalized;
        Vector2 origin = (Vector2)caster.transform.position + Vector2.up * 0.5f + direction * 2f;

        SkillController.Instance.isBowAttack = true;
        //SkillController.Instance.rangedAttack.PlayComboEffect();
        SystemManager.Instance.coroutinRunner.RunCoroutine(ExecuteWithEffectDelay(caster, origin, direction, castData));
    }

    private IEnumerator ExecuteWithEffectDelay(GameObject caster, Vector2 origin, Vector2 direction, SkillCastData castData)
    {
        //PlaySound("BowShoot2");
        GameManager.Instance.cameraZoom.ZoomIn(0.5f);  
        yield return new WaitForSeconds(damageDelay);
        SkillController.Instance.rangedAttack.AdvanceCombo();
        SkillController.Instance.rangedAttack.PlayShotEffect();
        GameManager.Instance.cameraZoom.ZoomOut();
        float extraLength = 3f; // 범위 확장값
        Vector2 dir = direction.normalized;

        float totalRange = range + extraLength; // 전체 박스 길이

        Vector2 center = origin + dir * (range / 2f); // origin 기준 앞으로 절반만큼 이동한 지점을 중심으로
        Vector2 size = new Vector2(totalRange, hitRadius * 2f); // 박스 크기
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // OverlapBox로 판정
        var hits = Physics2D.OverlapBoxAll(center, size, angle, hitMask);

        foreach (var hit in hits)
        {
            DealDamageToTarget(hit.gameObject, castData);  // 데미지 처리
            Debug.Log($"Hit {hit.name}");
            GameManager.Instance.cameraShake.Shake(0.1f, 0.2f);  // 카메라 쉐이크
            SkillController.Instance.isBowAttack = false;
        }

        DebugDrawUtil.DrawBox(center, size, angle, Color.red, 0.5f);
    }

    public override SkillExecutionSO Clone()
    {
        BowSkill01ExecutionSO clone = CreateInstance<BowSkill01ExecutionSO>();

        clone.range = this.range;
        clone.hitMask = this.hitMask;
        clone.damageDelay = this.damageDelay;
        clone.hitRadius = this.hitRadius;

        return clone;
    }
}
