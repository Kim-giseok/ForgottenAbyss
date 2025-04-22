using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkillController : Singleton<SkillController>
{
    public ComboAttack comboAttack;
    public RangedAttack rangedAttack;

    public int combatId;
    public int skill01Id;
    public int skill02Id;

    public Transform skillSpawnPoint;
    public Transform skillSpawnPoint2;

    private bool isGettingHit = false;
    private bool isDead = false;
    private bool isSkillPlaying = false;
    public bool isBowAttack = false;

    private ComboAttack Combo
    {
        get
        {
            if (comboAttack == null)
                comboAttack = FindObjectOfType<ComboAttack>();
            return comboAttack;
        }
    }

    private RangedAttack Ranged
    {
        get
        {
            if (rangedAttack == null)
                rangedAttack = FindObjectOfType<RangedAttack>();
            return rangedAttack;
        }
    }

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
    }

    private void Update()
    {
        if (ActionBufferUtil.Instance != null)
            ActionBufferUtil.Instance.Update();
    }

    void OnAttack(InputValue value)
    {
        if (isSkillPlaying || isGettingHit || isDead) return;

        var weaponData = WeaponManager.Instance.GetCurrentWeaponData();
        if (weaponData == null)
        {
            Debug.LogWarning("기본 공격: 무기가 장착되어 있지 않음.");
            return;
        }

        if (IsTurning())
        {
            ActionBufferUtil.Instance.BufferAction(
                "NormalAttack",
                () => !IsTurning() && !isSkillPlaying,
                () =>
                {
                    if (weaponData.Type == WeaponType.Sword)
                    {
                        if (Combo != null)
                            Combo.HandleAttackInput();
                    }
                    else if (weaponData.Type == WeaponType.Bow)
                    {
                        if (Ranged != null)
                            Ranged.HandleAttackInput();
                    }
                });
        }
        else
        {
            if (weaponData.Type == WeaponType.Sword)
            {
                if (Combo != null)
                    Combo.HandleAttackInput();
            }
            else if (weaponData.Type == WeaponType.Bow)
            {
                if (Ranged != null)
                    Ranged.HandleAttackInput();
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

        var memoryPieceSO = WeaponManager.Instance.GetCurrentMemoryPieceSO();
        if (memoryPieceSO == null || memoryPieceSO.skillItem == null)
        {
            Debug.LogWarning("기억 스킬 없음 또는 ID 비정상");
            return;
        }

        int memorySkillId = memoryPieceSO.skillItem.memoryPieceId;

        TryBufferOrExecuteSkill(memorySkillId, "SpecialSkill");
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
        return (Combo != null && Combo.IsAttacking) ||
               (Ranged != null && Ranged.IsAttacking) ||
               isSkillPlaying;
    }

    IEnumerator UseSkillRoutine(int skillId)
    {
        isSkillPlaying = true;

        var spawnPoint = WeaponManager.Instance.GetCurrentWeaponData().Type == WeaponType.Sword ? skillSpawnPoint : skillSpawnPoint2;
        SkillManager.Instance.TryUseSkill(skillId, spawnPoint);

        if (SkillManager.Instance.IsMemorySkill(skillId))
        {
            GameManager.Instance.player.controller.isInvincible = true;
            yield return new WaitForSeconds(3.0f);
            GameManager.Instance.player.controller.isInvincible = false;
        }
        else
        {
            yield return new WaitForSeconds(GetAnimPlayTime(skillId));
        }

        isSkillPlaying = false;
    }

    public float GetAnimPlayTime(int skillId)
    {
        if (SkillManager.Instance.IsMemorySkill(skillId))
            return 1.0f;

        var instance = SkillManager.Instance.GetSkillInstance(skillId);
        if (instance == null || instance.visual == null || instance.visual.animationSpeed <= 0f)
            return 0.5f;

        return instance.visual.animPlayTime / instance.visual.animationSpeed;
    }

    private void TryBufferOrExecuteSkill(int skillId, string bufferName)
    {
        if (!SkillManager.Instance.IsSkillEquipped(skillId))
        {
            Debug.LogWarning($"Skill ID {skillId} is not equipped.");
            return;
        }

        if (IsAttacking()) return;

        if (IsTurning())
        {
            ActionBufferUtil.Instance.BufferAction(
                bufferName,
                () => !IsTurning() && !IsAttacking(),
                () => StartCoroutine(UseSkillRoutine(skillId)));
        }
        else
        {
            StartCoroutine(UseSkillRoutine(skillId));
        }
    }

    public void SetGettingHit(bool value) => isGettingHit = value;
    public void SetDead(bool value) => isDead = value;

    public void ResetAttack()
    {
        isSkillPlaying = false;

        if (Combo != null && Combo.gameObject.activeInHierarchy)
        {
            Combo.EndComboAttack();
        }
        else
        {
            comboAttack = FindObjectOfType<ComboAttack>();
            Debug.LogWarning("comboAttack이 null이거나 Destroy됨 → 재연결 시도");
        }

        if (Ranged != null && Ranged.gameObject.activeInHierarchy)
        {
            Ranged.EndRangedAttack();
        }
        else
        {
            rangedAttack = FindObjectOfType<RangedAttack>();
            Debug.LogWarning("rangedAttack이 null이거나 Destroy됨 → 재연결 시도");
        }
    }
}
