using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public enum SkillSourceType { Weapon, Memory }

public class SkillInstance
{
    public int skillId;
    public SkillSourceType sourceType;

    public SkillData data;                   // 무기 스킬용
    public SkillVisualSO visual;            // 공통 비주얼
    public SkillExecutionSO execution;      // 무기 스킬용

    public MemoryPieceSO memorySO;      // 기억 스킬용

    public SkillInstance(SkillData data)
    {
        sourceType = SkillSourceType.Weapon;
        this.data = data;
        skillId = data.Id;

        visual = DataManager.Instance.GetSkillVisualSO(data.VisualSOName);
        execution = DataManager.Instance.GetSkillExecutionSO(data.ExecutionSOName);
    }

    public SkillInstance(MemoryPieceSO memorySO)
    {
        sourceType = SkillSourceType.Memory;
        this.memorySO = memorySO;
        skillId = memorySO.currentMemoryPieceId;

        // 이펙트 등 추가하고 싶다면 여기서 SO 따로 부여 가능
        // 예: visual = memoryItem.skillVisualSO;
    }

    public void Execute(GameObject caster)
    {
        switch (sourceType)
        {
            case SkillSourceType.Weapon:
                execution?.Execute(caster, null, data);
                break;
            case SkillSourceType.Memory:
                memorySO?.skillItem?.Use();
                break;
        }
    }

    public void PlayAnimation(Animator animator)
    {
        if (visual == null || animator == null) return;

        animator.SetBool("IsAttacking", true);
        animator.speed = visual.animationSpeed;

        if (!string.IsNullOrEmpty(visual.animationName))
            animator.Play(visual.animationName);
    }

    public IEnumerator ResetAnimator(Animator animator)
    {
        if (visual == null || animator == null) yield break;

        yield return new WaitForSeconds(visual.resetTime);
        animator.speed = 1f;
        animator.SetBool("IsAttacking", false);
    }

    public IEnumerator PlayEffect(Transform spawnPoint)
    {
        if (visual == null || visual.skillEffectPrefab == null || visual.isTogether) yield break;

        if (visual.effectDelay > 0f)
            yield return new WaitForSeconds(visual.effectDelay);

        Vector3 dir = spawnPoint.right;
        Vector3 pos = spawnPoint.position + dir * visual.effectXOffset + Vector3.up * visual.effectYOffset;

        GameObject effect = EffectPool.Instance.SpawnEffect(visual.effectKey, pos, spawnPoint.rotation);

        switch (WeaponManager.Instance.GetCurrentWeaponData().Type)
        {
            case WeaponType.Bow:
                effect.GetComponent<PiercingArrowEffect>()?.Initialize(pos, dir);
                break;
            case WeaponType.Sword:
                effect.GetComponent<DashTrailEffect>()?.Initialize(pos, dir);
                break;
        }
    }

    public float GetCooldown()
    {
        return sourceType switch
        {
            SkillSourceType.Weapon => data?.CoolTime ?? 0f,
            SkillSourceType.Memory => memorySO?.skillItem?.coolTime ?? 0f,
            _ => 0f
        };
    }

    public Sprite GetIcon()
    {
        return sourceType switch
        {
            SkillSourceType.Weapon => visual?.skillIcon,
            SkillSourceType.Memory => memorySO?.icon,
            _ => null
        };
    }
}
