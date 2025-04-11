using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow_Skill01_Execution", menuName = "SO/Skill/Execution/BowSkill01")]
public class BowSkill01ExecutionSO : SkillExecutionSO
{
    float range = 5f;
    public LayerMask hitMask;

    public override void Execute(GameObject caster, GameObject target, SkillData data)
    {
        //caster.GetComponent<MonoBehaviour>().StartCoroutine(PlayFastAnimation(caster, "BowAttack", 1.0f, 0.5f));

        SkillCastData castData = PrepareCastData(caster, target, data);

        Vector2 origin = caster.transform.position;
        Vector2 direction = caster.transform.right;

        // 관통 판정 (RaycastAll)
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, range, hitMask);

        foreach (var hit in hits)
        {
            DealDamageToTarget(hit.collider.gameObject, castData);
        }
    }
}
