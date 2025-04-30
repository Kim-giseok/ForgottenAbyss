using UnityEngine;
using System.Collections;

public enum SkillSourceType { Weapon, Memory }

public class SkillInstance
{
    public int skillId;
    public SkillSourceType sourceType;

    public SkillData data;
    public SkillVisualSO visual;            // 공통 비주얼
    public SkillExecutionSO execution;      // 실행

    public MemoryPieceSO memorySO;          // 기억 스킬용

    public SkillInstance(SkillData data)
    {
        sourceType = SkillSourceType.Weapon;
        this.data = data;
        skillId = data.Id;

        visual = SystemManager.Instance.dataManager.GetSkillVisualSO(data.VisualSOName);
        execution = SystemManager.Instance.dataManager.GetSkillExecutionSO(data.ExecutionSOName);
    }

    public SkillInstance(MemoryPieceSO memorySO)
    {
        sourceType = SkillSourceType.Memory;
        this.memorySO = memorySO;
        skillId = memorySO.currentMemoryPieceId;
    }

    public SkillInstance Clone()
    {
        SkillInstance clone;

        if (sourceType == SkillSourceType.Weapon)
        {
            SkillData clonedData = data.Clone();
            SkillVisualSO clonedVisual = visual != null ? visual.Clone() : null;
            SkillExecutionSO clonedExecution = execution != null ? execution.Clone() : null;

            clone = new SkillInstance(clonedData)
            {
                visual = clonedVisual,
                execution = clonedExecution
            };
        }
        else
        {
            MemoryPieceSO clonedMemorySO = memorySO != null ? memorySO : null;

            clone = new SkillInstance(clonedMemorySO)
            {
                memorySO = clonedMemorySO
            };
        }

        return clone;
    }

    public void Execute(GameObject caster)
    {
        Execute(caster, null);
    }

    public void Execute(GameObject caster, GameObject target)
    {
        switch (sourceType)
        {
            case SkillSourceType.Weapon:
                if (execution != null && data != null)
                    execution.Execute(caster, target, data);
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

        GameObject effect = SystemManager.Instance.effect.SpawnEffect(visual.effectKey, pos, spawnPoint.rotation);

        var weaponData = SystemManager.Instance.weaponManager.GetCurrentWeaponData();
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

    public int GetMpCost()
    {
        switch (sourceType)
        {
            case SkillSourceType.Weapon:
                return data != null ? data.MpCost : 0;
            case SkillSourceType.Memory:
                return memorySO != null && memorySO.skillItem != null
                    ? memorySO.skillItem.MpCost : 0;
            default:
                return 0;
        }
    }

    // 여기에 쿨감 적용
    public float GetCooldown()
    {
        // 기본 쿨타임
        float baseCd = 0f;

        if (sourceType == SkillSourceType.Weapon)
            baseCd = data != null ? data.CoolTime : 0f;
        else if (sourceType == SkillSourceType.Memory && memorySO != null && memorySO.skillItem != null)
            baseCd = memorySO.skillItem.coolTime;

        var status = GameManager.Instance.player.GetComponent<PlayerStatus>();

        float coolReductionPercent = status != null
            ? status.GetStat(StatType.COOLDOWN_REDUCTION)  // < 쿨감 적용시
            : 0f;

        // 쿨감 비율 계산(제한 최대 50%)
        float coolReductionRatio = Mathf.Clamp(coolReductionPercent / 100f, 0f, 0.5f);

        // 최종 쿨타임 계산 : 기본 쿨타임 * (1 - 쿨감 비율)
        float finalCd = baseCd * (1f - coolReductionRatio);
        return finalCd;
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
