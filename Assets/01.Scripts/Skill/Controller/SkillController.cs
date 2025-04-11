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

    private bool isSkillPlaying = false;

    void OnAttack(InputValue value)
    {
        if (isSkillPlaying) return;

        if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Sword)
        {
            comboAttack?.HandleAttackInput();
        }
        else if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Bow)
        {
            if (value.isPressed)
                rangedAttack?.HandleAttackInput();
            else
                rangedAttack?.HandleAttackInput();
        }

        Debug.Log("A: 일반공격");
    }

    void OnFirstSkill(InputValue value)
    {
        if (isSkillPlaying) return;

        StartCoroutine(UseSkillRoutine(skill01Id));
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill(InputValue value)
    {
        if (isSkillPlaying) return;

        StartCoroutine(UseSkillRoutine(skill02Id));
        Debug.Log("D: 스킬2");
    }

    void OnSpecialSkill(InputValue value) //특수스킬 키 입력
    {
        if (isSkillPlaying) return;

        StartCoroutine(UseSkillRoutine(memorySkillId));
        Debug.Log("R: 기억 스킬");
    }

    public void SetSkillPlaying(bool value)
    {
        isSkillPlaying = value;
    }

    IEnumerator UseSkillRoutine(int skillId)
    {
        isSkillPlaying = true;

        SkillManager.Instance.TryUseSkill(skillId, skillSpawnPoint);

        yield return new WaitForSeconds(1.0f); //스킬 연출 시간

        isSkillPlaying = false;
    }
}
