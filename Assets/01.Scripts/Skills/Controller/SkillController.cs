using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public int testSkillId = 1; // 테스트용 스킬 ID
    public Transform skillSpawnPoint; // 이펙트를 생성할 위치

    private float nextAvailableTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUseSkill(testSkillId);
        }
    }

    void TryUseSkill(int skillId)
    {
        var skillData = DataManager.Instance.GetSkillData(skillId);
        if (skillData == null)
        {
            Debug.LogError($"SkillData with ID {skillId} not found!");
            return;
        }
        if (Time.time < nextAvailableTime)
        {
            Debug.Log($"Skill {skillData.Name} is on cooldown.");
            return;
        }

        var skillSO = DataManager.Instance.GetSkillSO(skillData.SkillSOName);
        if (skillSO == null || skillSO.skillEffectPrefab == null)
        {
            Debug.LogWarning("Skill SO or Effect Prefab is missing.");
            return;
        }

        // 스킬 발동
        Instantiate(skillSO.skillEffectPrefab, skillSpawnPoint.position, skillSpawnPoint.rotation);
        Debug.Log($"Used Skill: {skillData.Name} for {skillData.Damage} damage!");

        // 쿨타임 갱신
        nextAvailableTime = Time.time + skillData.CoolTime;
    }
}
