using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : MonoBehaviour
{
    public ComboAttack comboAttack;
    public RangedAttack rangedAttack;

    public int combatId;
    public int skill01Id;
    public int skill02Id;
    public int memorySkillId;

    public Transform skillSpawnPoint;

    void OnAttack(InputValue value)
    {
        comboAttack.HandleAttackInput();
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

    void OnSpecialSkill(InputValue value) //특수스킬 키 입력
    {
        SkillManager.Instance.TryUseSkill(memorySkillId, skillSpawnPoint);
        Debug.Log("R: 기억 스킬");
    }
}
