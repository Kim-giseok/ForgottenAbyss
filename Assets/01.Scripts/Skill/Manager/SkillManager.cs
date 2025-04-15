using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private Dictionary<int, float> nextAvailableTimes = new Dictionary<int, float>();

    private List<int> currentWeaponSkillIds = new List<int>();
    private int currentMemorySkillId = -1;

    public SkillUI skillUI;

    // 무기 장착 시 호출
    public void SetCurrentWeaponSkills(int weaponId)
    {
        var weaponData = DataManager.Instance.GetWeaponData(weaponId);
        if (weaponData == null)
        {
            Debug.LogWarning($"WeaponData not found for ID {weaponId}");
            return;
        }

        currentWeaponSkillIds = new List<int>
        {
        weaponData.ComboAttack,
        weaponData.Skill1Id,
        weaponData.Skill2Id
        };
    }

    // 기억 스킬 장착 시 호출
    public void SetMemorySkill(int skillId)
    {
        var data = DataManager.Instance.GetSkillData(skillId);
        if (data == null)
        {
            Debug.LogError($"Memory SkillData not found for ID: {skillId}");
            return;
        }

        currentMemorySkillId = skillId;
        Debug.Log($"Memory Skill Set: {data.Name}");
    }

    public SkillData GetCurrentMemorySkillData()
    {
        if (currentMemorySkillId == -1) return null;
        return DataManager.Instance.GetSkillData(currentMemorySkillId);
    }

    public void TryUseSkill(int skillId, Transform spawnPoint)
    {
        // 0. 장착 여부 확인
        bool isMemorySkill = skillId == currentMemorySkillId;
        bool isWeaponSkill = currentWeaponSkillIds.Contains(skillId);

        if (!isMemorySkill && !isWeaponSkill)
        {
            Debug.LogWarning($"Skill ID {skillId} is not equipped.");
            return;
        }

        // 1. 스킬 데이터
        var skillData = DataManager.Instance.GetSkillData(skillId);
        if (skillData == null)
        {
            Debug.LogError($"SkillData with ID {skillId} not found!");
            return;
        }

        // 2. 쿨타임 검사
        if (nextAvailableTimes.TryGetValue(skillId, out float nextTime) && Time.time < nextTime)
        {
            float remain = nextTime - Time.time;
            Debug.Log($"{skillData.Name} is on cooldown. {remain:F1} sec remaining.");
            return;
        }

        // 3. 비주얼 SO
        var visualSO = DataManager.Instance.GetSkillVisualSO(skillData.VisualSOName);
        if (visualSO != null)
        {
            // 애니메이션 처리
            Animator anim = spawnPoint.GetComponentInParent<Animator>();
            if (anim != null)
            {
                anim.SetBool("IsAttacking", true);
                anim.speed = visualSO.animationSpeed;

                if (!string.IsNullOrEmpty(visualSO.animationName))
                {
                    anim.Play(visualSO.animationName);
                }

                StartCoroutine(ResetAnimatorSpeed(anim, visualSO.resetTime));
            }

            // 이펙트 처리
            if (visualSO.skillEffectPrefab != null && !visualSO.isTogether)
            {
                StartCoroutine(PlayEffectWithDelay(visualSO, spawnPoint));
            }
        }

        // 4. 실행 SO
        var executionSO = DataManager.Instance.GetSkillExecutionSO(skillData.ExecutionSOName);
        if (executionSO != null)
        {
            executionSO.Execute(spawnPoint.gameObject, null, skillData); // 타겟 지정 필요시 수정
        }
        else
        {
            Debug.LogWarning($"ExecutionSO not found for {skillData.ExecutionSOName}");
        }

        // 5. 쿨타임 갱신
        nextAvailableTimes[skillId] = Time.time + skillData.CoolTime;

        int slotIndex = GetSlotIndexBySkillId(skillId);
        if (slotIndex != -1)
        {
            skillUI.HideSkillSetting(slotIndex, skillData.CoolTime);
        }
    }

    private int GetSlotIndexBySkillId(int skillId)
    {
        var sc = WeaponManager.Instance.skillController;
        if (skillId == sc.combatId) return 1;
        if (skillId == sc.skill01Id) return 2;
        if (skillId == sc.skill02Id) return 3;
        if (skillId == sc.memorySkillId) return 0;
        return -1;
    }

    private IEnumerator ResetAnimatorSpeed(Animator anim, float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.speed = 1.0f;
        anim.SetBool("IsAttacking", false);
    }

    private IEnumerator PlayEffectWithDelay(SkillVisualSO visualSO, Transform spawnPoint)
    {
        if (visualSO.effectDelay > 0f)
            yield return new WaitForSeconds(visualSO.effectDelay);

        Vector3 dir = spawnPoint.right;
        Vector3 effectPos = spawnPoint.position;

        if (visualSO.useEffectOffset)
        {
            effectPos += dir * visualSO.effectXOffset;
            effectPos += Vector3.up * visualSO.effectYOffset;
        }

        GameObject effect = EffectPool.Instance.SpawnEffect(visualSO.effectKey, effectPos, spawnPoint.rotation);

        WeaponType currentType = WeaponManager.Instance.GetCurrentWeaponData().Type;
        if (currentType == WeaponType.Bow)
        {
            var pierceEffect = effect.GetComponent<PiercingArrowEffect>();
            if (pierceEffect != null)
            {
                pierceEffect.Initialize(effectPos, dir);
            }
        }
        else if (currentType == WeaponType.Sword)
        {
            var dashEffect = effect.GetComponent<DashTrailEffect>();
            if (dashEffect != null)
            {
                dashEffect.Initialize(effectPos, dir);
            }
        }
    }
}
