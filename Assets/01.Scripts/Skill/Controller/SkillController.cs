using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : Singleton<SkillController>
{
    public ComboAttack comboAttack;
    public RangedAttack rangedAttack;

    public int combatId;
    public int skill01Id;
    public int skill02Id;
    public MemorySkillItem memorySkillItem;

    public Transform skillSpawnPoint;
    public Transform skillSpawnPoint2;

    private bool isGettingHit = false;
    private bool isDead = false;
    private bool isSkillPlaying = false;
    public bool isBowAttack = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Init();
    }

    private void Update()
    {
        if(ActionBufferUtil.Instance != null)
            ActionBufferUtil.Instance.Update();

        //Debug.Log($"[DEBUG] combo: {comboAttack.IsAttacking}, ranged: {rangedAttack.IsAttacking}, skill: {isSkillPlaying}");
    }

    void OnAttack(InputValue value)
    {
        if (isSkillPlaying || isGettingHit || isDead) return;

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
        if (isGettingHit || isDead) return;
        TryBufferOrExecuteSkill(skill01Id, "FirstSkill");
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill(InputValue value)
    {
        if (isGettingHit || isDead) return;
        TryBufferOrExecuteSkill(skill02Id, "SecondSkill");
        Debug.Log("D: 스킬2");
    }

    void OnSpecialSkill(InputValue value)
    {
        if (isGettingHit || isDead) return;
        TryBufferOrExecuteSkill(memorySkillItem.memoryPieceId, "SpecialSkill");
        //memorySkillItem.Use();
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

        if (SkillManager.Instance.GetCurrentMemorySkillData()?.memoryPieceId == skillId)
        {
            yield return new WaitForSeconds(2.0f);
        }
        else
        {
            yield return new WaitForSeconds(GetAnimPlayTime(skillId));
        }

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

    public void SetGettingHit(bool value)
    {
        isGettingHit = value;
    }

    public void SetDead(bool value)
    {
        isDead = value;
    }

    public void ResetAttack()
    {
        Debug.Log("리셋 실행");

        isSkillPlaying = false;

        if (comboAttack != null && comboAttack.gameObject != null)
        {
            comboAttack.EndComboAttack();
        }
        else
        {
            Debug.LogWarning("comboAttack이 Destroy되어 null입니다.");
        }

        if (rangedAttack != null && rangedAttack.gameObject != null)
        {
            rangedAttack.EndRangedAttack();
        }
        else
        {
            Debug.LogWarning("rangedAttack이 Destroy되어 null입니다.");
        }
    }
}
