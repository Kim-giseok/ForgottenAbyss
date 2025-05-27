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

    public ControllerPlayer player;

    private bool isGettingHit = false;
    private bool isDead = false;
    public bool isSkillPlaying = false;
    public bool isBowAttack = false;
    
    [HideInInspector] public bool isDisable;
    
    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        var weaponManager = SystemManager.Instance.weaponManager;
        var skillManager = SystemManager.Instance.skillManager;
        player = GameManager.Instance.player.controller;

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

        Debug.Log($"[Combat] ���� Ÿ��: {weaponData.Type}, ���� �Ϸ�");

        if (skillManager != null)
        {
            skill01 = skillManager.GetSkillInstance(weaponData.Skill1Id);
            skill02 = skillManager.GetSkillInstance(weaponData.Skill2Id);

            if (skillManager.currentMemoryPiece != null)
            {
                var memoryId = skillManager.currentMemoryPiece.currentMemoryPieceId;
                memorySkill = skillManager.GetSkillInstance(memoryId);
            }

            Debug.Log("[SkillController] ��ų ���� �Ϸ�");
        }
    }

    private void Update()
    {
        if (SystemManager.Instance.actionBufferUtil != null)
            SystemManager.Instance.actionBufferUtil.Update();
    }

    void OnAttack(InputValue value)
    {
        if(isDisable) return;
        
        AnimatorStateInfo stateInfo = GameManager.Instance.player.animator.GetCurrentAnimatorStateInfo(0);
        bool isInAttackState = stateInfo.IsTag("Attack") && stateInfo.normalizedTime <= 0.1f;

        if (isInAttackState || isSkillPlaying || player.isOnLadder) return;

        if (!SystemManager.Instance.weaponManager.IsWeaponEquipped())
        {
            Debug.LogWarning("���Ⱑ �������� �ʾҽ��ϴ�. �Ϲݰ��� �Ұ�!!");
            return;
        }

        if (!IsExecutable() || !player.canAttack || IsTurning())
        {
            Debug.Log("A: ���� �Ұ� �Ǵ� ���� ��ȯ �� - ���� �Է� ���۸�");
            SystemManager.Instance.actionBufferUtil.BufferAction(
                "NormalAttack",
                () => IsExecutable() && player.canAttack && !isInAttackState,
                () => StartCoroutine(DelayedCombatExecution()));

            return;
        }
        else
            combatSkill.Execute();

        Debug.Log("A: �⺻ ����");
    }

    void OnFirstSkill(InputValue value)
    {
        if(isDisable) return;
        
        if ((!IsExecutable() && !IsBufferable()) || player.isOnLadder) return;
        TryBufferOrExecuteSkill(skill01, "FirstSkill");
        Debug.Log("S: ��ų1");
    }

    void OnSecondSkill(InputValue value)
    {
        if(isDisable) return;

        if ((!IsExecutable() && !IsBufferable()) || player.isOnLadder) return;
        TryBufferOrExecuteSkill(skill02, "SecondSkill");
        Debug.Log("D: ��ų2");
    }

    void OnSpecialSkill(InputValue value)
    {
        if(isDisable) return;

        if ((!IsExecutable() && !IsBufferable()) || player.isOnLadder) return;
        if (memorySkill == null)
        {
            if (DamageTextManager.Instance != null)
                DamageTextManager.Instance.ShowMessage("��� ��ų�� �������� �ʾҽ��ϴ�.");

            return;
        }
        TryBufferOrExecuteSkill(memorySkill, "SpecialSkill");
        Debug.Log("R: ��� ��ų");
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
        if (!player.canAttack || !player.canSkill || isGettingHit || isDead)
            return false;

        return true;
    }

    public bool IsBufferable()
    {
        if (!player.canAttack || !player.canSkill)
            return true;

        return false;
    }

    //public bool IsAttacking()
    //{
    //    // Ȱ�� �˺��� ������ �Ͱ��Ƽ� ��ų ��� ������ �ӽ÷� Ǯ���� ��Ÿ �� ��ų ��� ����
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
            player.isInvincible = true;
            yield return new WaitForSeconds(3.0f);
            player.isInvincible = false;
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
                DamageTextManager.Instance.ShowMessage("��ų�� �������� �ʾҽ��ϴ�!");

            return;
        }

        if (isSkillPlaying) return;

        if (IsExecutable())
        {
            Debug.Log("�ٷ� ����");
            StartCoroutine(UseSkillRoutine(instance));
        }
        else if (IsBufferable()) // ��� ���� �Ұ��������� ���� ����� ���ɼ��� �ִٸ� ���۸�
        {
            Debug.Log("���۸� ����");
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

        isSkillPlaying = false;

        if (SystemManager.Instance.weaponManager.GetCurrentWeaponData() != null) 
        {
            var currentWeaponType = SystemManager.Instance.weaponManager.GetCurrentWeaponData().Type;

            switch (currentWeaponType)
            {
                case WeaponType.Sword:
                    if (comboAttack != null && comboAttack.gameObject.activeInHierarchy)
                        comboAttack.EndComboAttack();
                    else
                    {
                        comboAttack = FindObjectOfType<ComboAttack>();
                        Debug.LogWarning("comboAttack�� null�̰ų� Destroy�� �� �翬�� �õ�");
                    }
                    break;

                case WeaponType.Bow:
                    if (rangedAttack != null && rangedAttack.gameObject.activeInHierarchy)
                        rangedAttack.EndRangedAttack();
                    else
                    {
                        rangedAttack = FindObjectOfType<RangedAttack>();
                        Debug.LogWarning("rangedAttack�� null�̰ų� Destroy�� �� �翬�� �õ�");
                    }
                    break;

                default:
                    Debug.LogWarning($"ResetAttack() - �� �� ���� ���� Ÿ��: {currentWeaponType}");
                    break;
            }
        }
    }
}
