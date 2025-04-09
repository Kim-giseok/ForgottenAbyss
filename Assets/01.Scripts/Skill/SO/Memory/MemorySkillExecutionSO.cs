using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Memory_Skill_Execution", menuName = "SO/Skill/Execution/MemorySkill")]
public class MemorySkillExecutionSO : SkillExecutionSO
{
    public float range = 3f;

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        Vector3 center = caster.transform.position;

        // SkillCastData 준비
        SkillCastData castData = PrepareCastData(caster, target, data);

        // 1. 범위 내 적 탐색
        Collider2D[] hits = GetEnemiesInRange(center, range, LayerMask.GetMask("Enemy"));

        foreach (var hit in hits)
        {
            DealDamageToTarget(hit.gameObject, castData);
        }

        // 2. 범위 이펙트 생성 (시각적 효과)
        var visualSO = DataManager.Instance.GetSkillVisualSO(data.VisualSOName);
        if (visualSO != null && !string.IsNullOrEmpty(visualSO.effectKey))
        {
            GameObject effect = EffectPool.Instance.SpawnEffect(visualSO.effectKey, center, Quaternion.identity);

            if (effect != null)
            {
                effect.transform.localScale = Vector3.one * (range * 2f);
            }
        }

        // 3. 로그 출력
        Debug.Log($"Memory Skill executed. Hit {hits.Length} enemies.");
    }
}
