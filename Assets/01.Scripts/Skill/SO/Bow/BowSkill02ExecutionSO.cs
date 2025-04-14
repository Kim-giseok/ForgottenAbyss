using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow_Skill02_Execution", menuName = "SO/Skill/Execution/BowSkill02")]
public class BowSkill02ExecutionSO : SkillExecutionSO
{
    public string skillEffectKey;
    LayerMask targetLayer;

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        SkillCastData castData = PrepareCastData(caster, target, data);

        var visualSO = DataManager.Instance.GetSkillVisualSO(data.VisualSOName);

        if (visualSO != null)
        {
            Transform spawnPoint = caster.transform; 
            CoroutineRunner.instance.StartCoroutine(PlayEffectWithDelay(visualSO, spawnPoint, data, castData));
        }
    }

    private IEnumerator PlayEffectWithDelay(SkillVisualSO visualSO, Transform spawnPoint, SkillData data, SkillCastData castData)
    {
        if (visualSO.effectDelay > 0f)
            yield return new WaitForSeconds(visualSO.effectDelay);

        int shotCount = 3;
        float angleBetweenShots = 10f;
        float delayBetweenShots = 0.05f;

        List<Transform> enemies = FindEnemiesAround(spawnPoint.position, 10f);

        for (int i = 0; i < shotCount; i++)
        {
            float angleOffset = (i - 1) * angleBetweenShots;

            Vector3 dir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * spawnPoint.right;
            Vector3 effectPos = spawnPoint.position;

            if (visualSO.useEffectOffset)
            {
                effectPos += dir * visualSO.effectXOffset;
                effectPos += Vector3.up * visualSO.effectYOffset;
            }

            GameObject effect = EffectPool.Instance.SpawnEffect(visualSO.effectKey, effectPos, Quaternion.LookRotation(dir));

            HomingArrowEffect homingArrowEffect = effect.GetComponent<HomingArrowEffect>();
            if (homingArrowEffect != null)
            {
                homingArrowEffect.Initialize(effectPos, dir, castData.caster, data);

                if (i < enemies.Count && enemies[i] != null)
                {
                    homingArrowEffect.SetManualTarget(enemies[i]);
                }
            }
            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    private List<Transform> FindEnemiesAround(Vector3 center, float radius)
    {
        Collider2D[] hits = GetEnemiesInRange(center, radius, targetLayer);
        List<Transform> result = new List<Transform>();

        foreach (var hit in hits)
        {
            result.Add(hit.transform);
        }

        return result;
    }
}
