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
    public bool isSkillPlaying = false;
    public bool isBowAttack = false;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        var weaponManager = SystemManager.Instance.weaponManager;
        var skillManager = SystemManager.Instance.skillManager;

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
        if (SystemManager.Instance.actionBufferUtil != null)
            SystemManager.Instance.actionBufferUtil.Update();
    }

    void OnAttack(InputValue value)
    {
        AnimatorStateInfo stateInfo = GameManager.Instance.player.animator.GetCurrentAnimatorStateInfo(0);
        bool isInAttackState = stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.1f && stateInfo.normalizedTime <= 0.3f;

        if (isInAttackState) return;

        if (!SystemManager.Instance.weaponManager.IsWeaponEquipped())
        {
            Debug.LogWarning("무기가 장착되지 않았습니다. 일반공격 불가!!");
            return;
        }

        if (!IsExecutable() || !GameManager.Instance.player.controller.canAttack || IsTurning())
        {
            Debug.Log("A: 공격 불가 또는 방향 전환 중 - 공격 입력 버퍼링");
            SystemManager.Instance.actionBufferUtil.BufferAction(
                "NormalAttack",
                () => IsExecutable() && GameManager.Instance.player.controller.canAttack && !isInAttackState,
                () => StartCoroutine(DelayedCombatExecution()));

            return;
        }
        else
            combatSkill.Execute();

        Debug.Log("A: 기본 공격");
    }

    void OnFirstSkill(InputValue value)
    {
        if (!IsExecutable() && !IsBufferable()) return;
        TryBufferOrExecuteSkill(skill01, "FirstSkill");
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill(InputValue value)
    {
        if (!IsExecutable() && !IsBufferable()) return;
        TryBufferOrExecuteSkill(skill02, "SecondSkill");
        Debug.Log("D: 스킬2");
    }

    void OnSpecialSkill(InputValue value)
    {
        if (!IsExecutable() && !IsBufferable()) return;
        if (memorySkill == null)
        {
            if (DamageTextManager.Instance != null)
                DamageTextManager.Instance.ShowMessage("기억 스킬이 장착되지 않았습니다.");

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
        return stateInfo.IsTag("Turn") || stateInfo.IsTag("Dash");
    }

    public bool IsExecutable()
    {
        var player = GameManager.Instance.player.controller;

        if(!player.canAttack || !player.canSkill || isGettingHit || isDead) return false;
        else return true;
    }

    public bool IsBufferable()
    {
        var player = GameManager.Instance.player.controller;

        if (!player.canAttack || !player.canSkill) return true;
        return false;
    }

    //public bool IsAttacking()
    //{
    //    // 활이 검보다 안좋은 것같아서 스킬 사용 제한을 임시로 풀어줌 평타 중 스킬 사용 가능
    //    return (comboAttack != null && comboAttack.IsAttacking) ||
    //           //(rangedAttack != null && rangedAttack.IsAttacking) ||
    //           isSkillPlaying;
    //}

    public bool IsBasicAttack()
    {
        return (comboAttack != null && comboAttack.IsAttacking) ||
               (rangedAttack != null && rangedAttack.IsAttacking);
    }

    public bool IsJumpAttack()
    {
        return (comboAttack != null && !comboAttack.canJumpAttack) ||
            (rangedAttack != null && !rangedAttack.canJumpAttack);
    }

    IEnumerator UseSkillRoutine(SkillInstance instance)
    {
        isSkillPlaying = true;

        var spawnPoint = SystemManager.Instance.weaponManager.GetCurrentWeaponData().Type == WeaponType.Sword ? skillSpawnPoint : skillSpawnPoint2;
        SystemManager.Instance.skillManager.TryUseSkill(instance, spawnPoint);

        if (SystemManager.Instance.skillManager.IsMemorySkill(instance))
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

    private IEnumerator DelayedUseSkillRoutine(SkillInstance instance)
    {
        yield return new WaitUntil(() => !IsTurning());
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UseSkillRoutine(instance));
    }

    private IEnumerator DelayedCombatExecution()
    {
        yield return new WaitUntil(() => !IsTurning());
        yield return new WaitForSeconds(0.1f);
        combatSkill.Execute();
    }

    public float GetAnimPlayTime(SkillInstance instance)
    {
        if (SystemManager.Instance.skillManager.IsMemorySkill(instance))
            return 1.0f;

        if (instance == null || instance.visual == null || instance.visual.animationSpeed <= 0f)
            return 0.5f;

        return instance.visual.animPlayTime / instance.visual.animationSpeed;
    }

    private void TryBufferOrExecuteSkill(SkillInstance instance, string bufferName)
    {
        if (!SystemManager.Instance.skillManager.IsSkillEquipped(instance))
        {
            if (DamageTextManager.Instance != null)
                DamageTextManager.Instance.ShowMessage("스킬이 장착되지 않았습니다!");

            return;
        }

        if (combatSkill.weaponType != WeaponType.Bow && isSkillPlaying) return;

        if (IsExecutable())
        {
            Debug.Log("바로 실행");
            StartCoroutine(UseSkillRoutine(instance));
        }
        else if (IsBufferable()) // 즉시 실행 불가능하지만 이후 실행될 가능성이 있다면 버퍼링
        {
            Debug.Log("버퍼링 실행");
            SystemManager.Instance.actionBufferUtil.BufferAction(
                bufferName,
                () => IsExecutable(),
                () => StartCoroutine(UseSkillRoutine(instance))
            );
        }
    }

    public void SetGettingHit(bool value) => isGettingHit = value;
    public void SetDead(bool value) => isDead = value;

    public void UpdateBasicIcon()
    {
        switch (combatSkill.weaponType)
        {
            case WeaponType.Bow:
                rangedAttack.SwapWeapon();
                break;
            case WeaponType.Sword:
                comboAttack.SwapWeapon();
                break;
        }
    }

    private void ResetJumpAttack()
    {
        if(comboAttack != null && rangedAttack != null)
        {
            comboAttack.canJumpAttack = true;
            rangedAttack.canJumpAttack = true;

            comboAttack.IsAttacking = false;
            rangedAttack.IsAttacking = false;
        }
    }

    public void ResetAttack()
    {
        ResetJumpAttack();

        if (SystemManager.Instance.weaponManager.GetCurrentWeaponData() != null) 
        {
            isSkillPlaying = false;

            var currentWeaponType = SystemManager.Instance.weaponManager.GetCurrentWeaponData().Type;

            switch (currentWeaponType)
            {
                case WeaponType.Sword:
                    if (comboAttack != null && comboAttack.gameObject.activeInHierarchy)
                        comboAttack.EndComboAttack();
                    else
                    {
                        comboAttack = FindObjectOfType<ComboAttack>();
                        Debug.LogWarning("comboAttack이 null이거나 Destroy됨 → 재연결 시도");
                    }
                    break;

                case WeaponType.Bow:
                    if (rangedAttack != null && rangedAttack.gameObject.activeInHierarchy)
                        rangedAttack.EndRangedAttack();
                    else
                    {
                        rangedAttack = FindObjectOfType<RangedAttack>();
                        Debug.LogWarning("rangedAttack이 null이거나 Destroy됨 → 재연결 시도");
                    }
                    break;

                default:
                    Debug.LogWarning($"ResetAttack() - 알 수 없는 무기 타입: {currentWeaponType}");
                    break;
            }
        }
    }
}
