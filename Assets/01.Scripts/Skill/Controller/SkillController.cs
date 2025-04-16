using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : Singleton<SkillController>
{
    public ComboAttack comboAttack;
    public RangedAttack rangedAttack;

    public int combatId;
    public int skill01Id;
    public int skill02Id;
    public int memorySkillId;

    public Transform skillSpawnPoint;
    public Transform skillSpawnPoint2;

    private bool isSkillPlaying = false;
    public bool isBowAttack = false;

    private void Update()
    {
        if(ActionBufferUtil.Instance != null)
            ActionBufferUtil.Instance.Update();
    }

    void OnAttack(InputValue value)
    {
        if (isSkillPlaying) return;

        if (IsTurning())
        {
            ActionBufferUtil.Instance.BufferAction(
                "NormalAttack",
                () => !IsTurning() && !isSkillPlaying,
                () =>
                {
                    if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Sword)
                    {
                        comboAttack?.HandleAttackInput();
                    }
                    else if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Bow)
                    {
                        rangedAttack?.HandleAttackInput();
                    }
                }
            );
        }
        else
        {
            if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Sword)
            {
                comboAttack?.HandleAttackInput();
            }
            else if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Bow)
            {
                rangedAttack?.HandleAttackInput();
            }
        }

        Debug.Log("A: 일반공격");
    }

    void OnFirstSkill(InputValue value)
    {
        TryBufferOrExecuteSkill(skill01Id, "FirstSkill");
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill(InputValue value)
    {
        TryBufferOrExecuteSkill(skill02Id, "SecondSkill");
        Debug.Log("D: 스킬2");
    }

    void OnSpecialSkill(InputValue value)
    {
        TryBufferOrExecuteSkill(memorySkillId, "SpecialSkill");
        Debug.Log("R: 기억 스킬");
    }

    public void SetSkillPlaying(bool value)
    {
        isSkillPlaying = value;
    }

    public void OnSetSkillFalse()
    {
        isSkillPlaying = false;
    }

    public bool IsTurning()
    {
        AnimatorStateInfo stateInfo = GameManager.Instance.player.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("Turn") || stateInfo.IsTag("Fall"))
            return true;
        else return false;
    }

    public bool IsAttacking()
    {
        return comboAttack.IsAttacking || rangedAttack.IsAttacking || isSkillPlaying;
    }

    IEnumerator UseSkillRoutine(int skillId)
    {
        isSkillPlaying = true;

        if (WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Sword)
        {
            SkillManager.Instance.TryUseSkill(skillId, skillSpawnPoint);
        }

        else if(WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Bow)
        {
            SkillManager.Instance.TryUseSkill(skillId, skillSpawnPoint2);
        }

        yield return new WaitForSeconds(GetAnimPlayTime(skillId));

        isSkillPlaying = false;
    }

    public float GetAnimPlayTime(int skillId)
    {
        SkillData skilldata = DataManager.Instance.GetSkillData(skillId);
        SkillVisualSO skillVisual = DataManager.Instance.GetSkillVisualSO(skilldata.Name + "_Visual");

        if (skillVisual == null || skillVisual.animationSpeed <= 0f)
        {
            Debug.LogWarning($"SkillVisualSO missing or invalid for skillId: {skillId}");
            return 0.5f;
        }

        return skillVisual.animPlayTime / skillVisual.animationSpeed;
    }

    private void TryBufferOrExecuteSkill(int skillId, string bufferName)
    {
        if (IsAttacking()) return;

        if (IsTurning())
        {
            ActionBufferUtil.Instance.BufferAction(
                bufferName,
                () => !IsTurning() && !IsAttacking(),
                () => StartCoroutine(UseSkillRoutine(skillId))
            );
        }
        else
        {
            StartCoroutine(UseSkillRoutine(skillId));
        }
    }
}
