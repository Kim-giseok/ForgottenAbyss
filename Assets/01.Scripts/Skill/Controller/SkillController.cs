using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public int testSkillId = 1; // 테스트용 스킬 ID
    public Transform skillSpawnPoint; // 이펙트를 생성할 위치

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SkillManager.Instance.TryUseSkill(testSkillId, skillSpawnPoint);
        }
    }
}
