using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class SkillController : Singleton<SkillController>
{
    public ComboAttack comboAttack;
    public RangedAttack rangedAttack;

    public CombatInstance combatSkill;
    public SkillInstance skill01;
    public SkillInstance skill02;
    public SkillInstance memorySkill;

    public Transform skillSpawnPoint;
    public Transform skillSpawnPoint2;

    private bool isGettingHit = false;
    private bool isDead = false;
    private bool isSkillPlaying = false;
    public bool isBowAttack = false;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        var weaponManager = WeaponManager.Instance;
        var skillManager = SkillManager.Instance;

        var weaponSO = weaponManager.GetCurrentWeaponSO();
        var weaponData = weaponManager.GetCurrentWeaponData();

        if (weaponSO == null || weaponData == null) return;

        switch (weaponData.Type)
        {
            case WeaponType.Sword:
                var combo = GetComponent<ComboAttack>();
                combatSkill = new CombatInstance(combo, weaponSO.comboAttackData).Clone() ;
                break;

            case WeaponType.Bow:
                var ranged = GetComponent<RangedAttack>();
                combatSkill = new CombatInstance(ranged, weaponSO.rangedAttackData).Clone();
                break;
        }

        Debug.Log($"[Combat] 무기 타입: {weaponData.Type}, 연결 완료");

        if (skillManager != null)
        {
            skill01 = skillManager.GetSkillInstance(weaponData.Skill1Id);
            skill02 = skillManager.GetSkillInstance(weaponData.Skill2Id);

            if (skillManager.currentMemoryPiece != null)
            {
                var memoryId = skillManager.currentMemoryPiece.currentMemoryPieceId;
                memorySkill = skillManager.GetSkillInstance(memoryId);
            }

            Debug.Log("[SkillController] 스킬 연결 완료");
        }
    }

    private void Update()
    {
        if (ActionBufferUtil.Instance != null)
            ActionBufferUtil.Instance.Update();
    }

    void OnAttack(InputValue value)
    {
        if (isSkillPlaying || isGettingHit || isDead) return;

        if (IsTurning())
        {
            ActionBufferUtil.Instance.BufferAction(
                "NormalAttack",
                () => !IsTurning() && !isSkillPlaying,
                () => combatSkill.Execute());
        }
        else
        {
            combatSkill.Execute();
        }

        Debug.Log("A: 기본 공격");
    }

    void OnFirstSkill(InputValue value)
    {
        if (isGettingHit || isDead) return;
        TryBufferOrExecuteSkill(skill01, "FirstSkill");
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill(InputValue value)
    {
        if (isGettingHit || isDead) return;
        TryBufferOrExecuteSkill(skill02, "SecondSkill");
        Debug.Log("D: 스킬2");
    }

    void OnSpecialSkill(InputValue value)
    {
        if (isGettingHit || isDead) return;
        if (memorySkill == null)
        {
            Debug.LogWarning("기억 스킬이 장착되지 않았습니다.");
            return;
        }
        TryBufferOrExecuteSkill(memorySkill, "SpecialSkill");
        Debug.Log("R: 기억 스킬");
    }

    public void SetSkillPlaying(bool value) => isSkillPlaying = value;
    public void OnSetSkillFalse() => isSkillPlaying = false;

    public bool IsTurning()
    {
        AnimatorStateInfo stateInfo = GameManager.Instance.player.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("Turn") || stateInfo.IsTag("Fall");
    }

    public bool IsAttacking()
    {
        return (comboAttack != null && comboAttack.IsAttacking) ||
               (rangedAttack != null && rangedAttack.IsAttacking) ||
               isSkillPlaying;
    }

    IEnumerator UseSkillRoutine(SkillInstance instance)
    {
        isSkillPlaying = true;

        var spawnPoint = WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Sword ? skillSpawnPoint : skillSpawnPoint2;
        SkillManager.Instance.TryUseSkill(instance, spawnPoint);

        if (SkillManager.Instance.IsMemorySkill(instance))
        {
            GameManager.Instance.player.controller.isInvincible = true;
            yield return new WaitForSeconds(3.0f);
            GameManager.Instance.player.controller.isInvincible = false;
        }
        else
        {
            yield return new WaitForSeconds(GetAnimPlayTime(instance));
        }

        isSkillPlaying = false;
    }

    public float GetAnimPlayTime(SkillInstance instance)
    {
        if (SkillManager.Instance.IsMemorySkill(instance))
            return 1.0f;

        if (instance == null || instance.visual == null || instance.visual.animationSpeed <= 0f)
            return 0.5f;

        return instance.visual.animPlayTime / instance.visual.animationSpeed;
    }

    private void TryBufferOrExecuteSkill(SkillInstance instance, string bufferName)
    {
        if (!SkillManager.Instance.IsSkillEquipped(instance))
        {
            Debug.LogWarning($"Skill ID {instance} is not equipped.");
            return;
        }

        if (IsAttacking()) return;

        if (IsTurning())
        {
            ActionBufferUtil.Instance.BufferAction(
                bufferName,
                () => !IsTurning() && !IsAttacking(),
                () => StartCoroutine(UseSkillRoutine(instance)));
        }
        else
        {
            StartCoroutine(UseSkillRoutine(instance));
        }
    }

    public void SetGettingHit(bool value) => isGettingHit = value;
    public void SetDead(bool value) => isDead = value;

    public void ResetAttack()
    {
        isSkillPlaying = false;

        if (comboAttack != null && comboAttack.gameObject.activeInHierarchy)
        {
            comboAttack.EndComboAttack();
        }
        else
        {
            comboAttack = FindObjectOfType<ComboAttack>();
            Debug.LogWarning("comboAttack이 null이거나 Destroy됨 → 재연결 시도");
        }

        if (rangedAttack != null && rangedAttack.gameObject.activeInHierarchy)
        {
            rangedAttack.EndRangedAttack();
        }
        else
        {
            rangedAttack = FindObjectOfType<RangedAttack>();
            Debug.LogWarning("rangedAttack이 null이거나 Destroy됨 → 재연결 시도");
        }
    }
}
