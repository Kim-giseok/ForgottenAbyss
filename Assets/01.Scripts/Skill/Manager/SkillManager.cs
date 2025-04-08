using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    // 스킬별 쿨타임 기록용
    private Dictionary<int, float> nextAvailableTimes = new Dictionary<int, float>();

    public void TryUseSkill(int skillId, Transform spawnPoint)
    {
        // 1. 스킬 데이터 가져오기
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

        // 3. Visual SO 가져오기
        var visualSO = DataManager.Instance.GetSkillVisualSO(skillData.VisualSOName);
        if (visualSO == null)
        {
            Debug.LogWarning("SkillVisualSO not found.");
            return;
        }

        // 4. 이펙트 생성 (시각적 효과)
        if (visualSO.skillEffectPrefab != null && skillData.Type != SkillType.Memory)
        {
            Vector3 dir = spawnPoint.right;
            Vector3 effectPos = spawnPoint.position;

            if (visualSO.useEffectOffset)
            {
                effectPos += dir * visualSO.effectOffset; // 왼쪽으로 오프셋 적용
            }

            EffectPool.Instance.SpawnEffect(visualSO.effectKey, effectPos, spawnPoint.rotation);
        }

        // 5. 실행 SO 실행
        var executionSO = DataManager.Instance.GetSkillExecutionSO(skillData.ExecutionSOName);
        if (executionSO != null)
        {
            executionSO.Execute(spawnPoint.gameObject, null); // target은 상황에 따라 설정 가능
        }
        else
        {
            Debug.LogWarning($"ExecutionSO not found for {skillData.ExecutionSOName}");
        }

        // 6. 쿨타임 갱신
        nextAvailableTimes[skillId] = Time.time + skillData.CoolTime;
    }
}
