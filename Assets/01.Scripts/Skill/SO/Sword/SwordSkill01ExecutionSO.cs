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

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        //caster.GetComponent<MonoBehaviour>().StartCoroutine(PlayFastAnimation(caster, "SwordAttack_4", 1.6f, 0.5f));

        var castData = PrepareCastData(caster, target, data);

        caster.GetComponent<MonoBehaviour>().StartCoroutine(HitTwiceCoroutine(caster, castData));
    }

    private IEnumerator HitTwiceCoroutine(GameObject caster, SkillCastData castData)
    {
        HitEnemies(caster, castData);
        yield return new WaitForSeconds(delayBetweenHits);
        HitEnemies(caster, castData);
    }

    private void HitEnemies(GameObject caster, SkillCastData castData)
    {
        Vector2 center = caster.transform.position;
        Collider2D[] hits = GetEnemiesInRange(center, range, targetLayer);

        foreach (var hit in hits)
        {
            Debug.Log($"Hit {hit.name}");
            CameraShake.Instance.Shake(0.1f, 0.2f);
            DealDamageToTarget(hit.gameObject, castData);
            KnockbackUtil.ApplyKnockback(hit.gameObject, caster.transform.position, 1f);
        }

        DebugDrawUtil.DrawCircle(center, range, Color.red);
    }
}
