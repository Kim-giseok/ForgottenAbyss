using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    // 스킬별 쿨타임 기록용
    private Dictionary<int, float> nextAvailableTimes = new Dictionary<int, float>();

    public void TryUseSkill(int skillId, Transform spawnPoint)
    {
        // 데이터 매니저에서 스킬 데이터 가져오기
        var skillData = DataManager.Instance.GetSkillData(skillId);
        if (skillData == null)
        {
            Debug.LogError($"SkillData with ID {skillId} not found!");
            return;
        }

        // 쿨타임 검사
        if (nextAvailableTimes.ContainsKey(skillId) && Time.time < nextAvailableTimes[skillId])
        {
            float remain = nextAvailableTimes[skillId] - Time.time;
            Debug.Log($"Skill {skillData.Name} is on cooldown. {remain:F1} sec remaining.");
            return;
        }

        // SO에서 스킬 이펙트 가져오기
        var skillSO = DataManager.Instance.GetSkillSO(skillData.SkillSOName);
        if (skillSO == null || skillSO.skillEffectPrefab == null)
        {
            Debug.LogWarning("Skill SO or skillEffectPrefab is missing.");
            return;
        }

        // 이펙트 생성
        Instantiate(skillSO.skillEffectPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log($"Used Skill: {skillData.Name} (Damage: {skillData.DamageMultiplier})");

        // 쿨타임 갱신
        nextAvailableTimes[skillId] = Time.time + skillData.CoolTime;
    }
}
