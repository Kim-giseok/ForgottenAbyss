using UnityEngine;
using System.Collections;

public enum SkillSourceType { Weapon, Memory }

public class SkillInstance
{
    public int skillId;
    public SkillSourceType sourceType;

    public SkillData data;
    public SkillVisualSO visual;            // 공통 비주얼
    public SkillExecutionSO execution;      // 스킬

    public MemoryPieceSO memorySO;          // 기억 스킬용

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
    }

    public void Execute(GameObject caster)
    {
        switch (sourceType)
        {
            case SkillSourceType.Weapon:
                if (execution != null && data != null)
                    execution.Execute(caster, null, data);
                break;
            case SkillSourceType.Memory:
                if (memorySO != null && memorySO.skillItem != null)
                    memorySO.skillItem.Use();
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

        var weaponData = WeaponManager.Instance.GetCurrentWeaponData();
        if (weaponData == null) yield break;

        switch (weaponData.Type)
        {
            case WeaponType.Bow:
                var arrow = effect.GetComponent<PiercingArrowEffect>();
                if (arrow != null) arrow.Initialize(pos, dir);
                break;
            case WeaponType.Sword:
                var trail = effect.GetComponent<DashTrailEffect>();
                if (trail != null) trail.Initialize(pos, dir);
                break;
        }
    }

    // 여기에 쿨감 적용
    public float GetCooldown()
    {
        if (sourceType == SkillSourceType.Weapon)
            return data != null ? data.CoolTime : 0f;

        if (sourceType == SkillSourceType.Memory && memorySO != null && memorySO.skillItem != null)
            return memorySO.skillItem.coolTime;

        return 0f;
    }

    public Sprite GetIcon()
    {
        if (sourceType == SkillSourceType.Weapon)
            return visual != null ? visual.skillIcon : null;

        if (sourceType == SkillSourceType.Memory && memorySO != null)
            return memorySO.icon;

        return null;
    }
}
