using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : MonoBehaviour
{
    public int basicAttackSkillId;
    public int skill01Id;
    public int skill02Id;
    public int memorySkillId;

    public Transform skillSpawnPoint;

    void OnAttack(InputValue value)
    {
        SkillManager.Instance.TryUseSkill(basicAttackSkillId, skillSpawnPoint);
        Debug.Log("A: 일반공격");
    }

    void OnFirstSkill(InputValue value)
    {
        SkillManager.Instance.TryUseSkill(skill01Id, skillSpawnPoint);
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill(InputValue value)
    {
        SkillManager.Instance.TryUseSkill(skill02Id, skillSpawnPoint);
        Debug.Log("D: 스킬2");
    }
}
