using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillExecutionSO", menuName = "SO/Skill/Execution")]
public abstract class SkillExecutionSO : ScriptableObject
{
    [TextArea]
    public string description;

    // 스킬 실행
    public abstract void Execute(GameObject caster, GameObject target, SkillData data);

    protected SkillCastData PrepareCastData(GameObject caster, GameObject target, SkillData data)
    {
        return SkillCastData.Create(caster, target, data);
    }

    protected void DealDamageToTarget(GameObject target, SkillCastData castData)
    {
        var enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            float damage = castData.CalculateDamage();
            enemy.GetDamage(damage);

            Vector3 worldPos = target.transform.position + Vector3.up * 1f;
            DamageTextManager.Instance.ShowDamage(worldPos, (int)damage);
        }
    }

    protected Collider2D[] GetEnemiesInRange(Vector3 center, float range, LayerMask layer)
    {
        return Physics2D.OverlapCircleAll(center, range, layer);
    }

    //protected IEnumerator PlayFastAnimation(GameObject caster, string animationName, float speed, float duration)
    //{
    //    var animator = caster.GetComponent<Animator>();
    //    if (animator == null) yield break;

    //    float originalSpeed = animator.speed;
    //    animator.speed = speed;
    //    animator.Play(animationName);
    //    yield return new WaitForSeconds(duration);
    //    animator.speed = originalSpeed;
    //}
}
