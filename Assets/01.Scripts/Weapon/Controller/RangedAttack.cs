using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static SummonSkillManager;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int maxCombo = 3;
    [SerializeField] private ComboBar comboBar;
    [SerializeField] private Animator shotAnimator;
    private ControllerPlayer player;
    private GameObjectPool projectilePool;
    private RangedAttackSO rangedData;
    private int attackIndex = 0;
    public bool canJumpAttack = true;
    private bool canNextCombo = false;
    private bool inputCombo = false;
    private bool comboReady = false;

    public bool IsAttacking = false;

    //private bool isSkill = false;

    private void Start()
    {
        StartCoroutine(WaitForGameManagerReady());
    }

    private IEnumerator WaitForGameManagerReady()
    {
        yield return new WaitUntil(() => GameManager.Instance != null && GameManager.Instance.player.controller != null);
        player = GameManager.Instance.player.controller;
    }

    public void SetRangedAttackData(RangedAttackSO data)
    {
        rangedData = data;
        attackIndex = 0;
        IsAttacking = false;
    }

    public void HandleAttackInput()
    {
        if (rangedData == null) return;

        if (!player.isGround)
        {
            if (!IsAttacking)
                StartJumpAttack();
            return;
        }

        if (Time.time - player.lastJumpTime < 0.1f)
            return;

        if (IsAttacking)
        {
            if (canNextCombo)
            {
                inputCombo = true;
            }      
        }
        else
        {
            //if (comboReady)
            //{
            //    comboReady = false;
            //    IsAttacking = true;
            //    isSkill = false;
            //    animator.SetInteger("BowCombo", attackIndex);
            //    UpdateRangedAttackUI(attackIndex - 1);
            //    comboBar.PlayEffect();
            //    PlayRangedAnimation();
            //}
            //else
                StartRangedAttack();
        }
    }

    private void StartRangedAttack()
    {
        canJumpAttack = false;
        IsAttacking = true;
        attackIndex = 1;
        animator.ResetTrigger("BowTrigger");
        animator.SetTrigger("BowTrigger");
        animator.SetInteger("BowCombo", attackIndex);
        UpdateRangedAttackUI(attackIndex-1);
    }

    private void StartJumpAttack()
    {
        if (canJumpAttack == true)
        {
            canJumpAttack = false;
            IsAttacking = true;
            attackIndex = 3;
            animator.ResetTrigger("BowJumpTrigger");
            animator.SetTrigger("BowJumpTrigger");
        }
        else
            Debug.Log("ÀÌ¹Ì Á¡ÇÁ °ø°Ý ½ÇÇà Áß...");
    }

    void OnEndBowJumpAttack()
    {
        canJumpAttack = true;
        IsAttacking = false;
        attackIndex = 1;
    }

    private void PlayRangedAnimation()
    {
        string animName = rangedData.rangedSteps[attackIndex - 1].animationName;
        animator.Play(animName);
    }

    public void OnRangedCheck(float bufferTime)
    {
        canNextCombo = true;
        comboBar.StartCombo(bufferTime);

        StartCoroutine(RangedInputBuffer(bufferTime));
    }

    IEnumerator RangedInputBuffer(float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;

            if (inputCombo)
            {
                OnRangedNext();
                yield break;
            }

            yield return null;
        }

        canNextCombo = false;
    }

    public void OnRangedNext()
    {
        if (inputCombo && attackIndex < maxCombo)
        {
            inputCombo = false;

            if (!comboReady)
                attackIndex++;

            animator.SetInteger("BowCombo", attackIndex);
            UpdateRangedAttackUI(attackIndex-1);
            comboBar.PlayEffect();
            PlayRangedAnimation();
        }
        else
        {
            EndRangedAttack();
        }
    }

    void OnRangedReset()
    {
        EndRangedAttack();
    }

    public void OnFireProjectile()
    {
        if (rangedData == null || rangedData.rangedSteps.Count < attackIndex || attackIndex <= 0) return;

        RangedComboStep step = rangedData.rangedSteps[attackIndex - 1];

        StartCoroutine(FireProjectiles(step));
    }

    IEnumerator FireProjectiles(RangedComboStep step)
    {
        Vector3 baseDirection = firePoint.right;

        for (int i = 0; i < step.projectileCount; i++)
        {
            Vector3 direction = baseDirection;

            if (step.isSpread)
            {
                float angle = step.spreadAngle * (i - (step.projectileCount - 1) / 2f);
                direction = Quaternion.Euler(0, 0, angle) * baseDirection;
            }

            shotAnimator.SetTrigger("ShotTrigger");
            SpawnProjectile(direction);

            yield return new WaitForSeconds(step.fireDelay);
        }
    }

    void onKnockBack(float distance)
    {
        if (!player.isGround) return;

        StartCoroutine(MoveKnockBackCoroutine(distance));
    }

    IEnumerator MoveKnockBackCoroutine(float distance)
    {
        float moveTime = 0.1f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position - (transform.right * distance);

        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveTime);
            yield return null;
        }

        transform.position = targetPos; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡ ï¿½ï¿½ï¿½ï¿½
    }

    void SpawnProjectile(Vector3 direction)
    {
        SoundManager.Instance.Playsfx("BowAttack1");
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        GameObject projectile = SystemManager.Instance.projectile.Get(firePoint.position, rot);

        PlayerProjectile pp = projectile.GetComponent<PlayerProjectile>();
        if (pp != null)
        {
            int comboStepIndex = attackIndex - 1;

            if (!canJumpAttack) comboStepIndex = 0;

            if (comboStepIndex >= 0 && comboStepIndex < rangedData.rangedSteps.Count)
            {
                float multiplier = rangedData.rangedSteps[comboStepIndex].multiplier;
                pp.Setup(direction, caster: this.gameObject, multiplier, attackIndex);
            }
            else
            {
                Debug.LogWarning($"[RangedAttack] Invalid combo step index: {comboStepIndex}, attackIndex: {attackIndex}, total steps: {rangedData.rangedSteps.Count}");
            }
        }
    }

    public void SwapWeapon()
    {
        UpdateRangedAttackUI(0);
    }

    private void UpdateRangedAttackUI(int stepIndex)
    {
        if (rangedData != null && stepIndex >= 0 && stepIndex < rangedData.rangedSteps.Count)
        {
            Sprite newIcon = rangedData.rangedSteps[stepIndex].stepIcon;

            if (SystemManager.Instance.skillManager.skillUI != null)
            {
                SystemManager.Instance.skillManager.skillUI.SetSkillIcon(SkillSlotType.Basic, newIcon);
            }
        }
    }

    public void EndRangedAttack()
    {
        if (this == null || animator == null || !gameObject.activeInHierarchy)
            return;

        canJumpAttack = true;
        IsAttacking = false;
        canNextCombo = false;

        if (inputCombo)
        {
            inputCombo = false;
            StartCoroutine(RestartRangedComboAfterDelay(0.1f));
        }
        else
        {
            attackIndex = 1;
            animator.SetInteger("BowCombo", 0);
            animator.Play("Idle");

            Debug.Log("ÃÊ±âÈ­");

            if (SystemManager.Instance.weaponManager.GetCurrentWeaponData().Type == WeaponType.Bow)
                UpdateRangedAttackUI(attackIndex-1);
        }
    }

    private IEnumerator RestartRangedComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        attackIndex = 1;
        canJumpAttack = true;
        IsAttacking = true;
        canNextCombo = true;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(rangedData.rangedSteps[attackIndex - 1].animationName))
        {
            animator.ResetTrigger("BowTrigger");
            animator.SetTrigger("BowTrigger");
            animator.SetInteger("BowCombo", attackIndex);
            comboBar.PlayEffect();
            UpdateRangedAttackUI(attackIndex - 1);
        }
    }

    public void PlayShotEffect()
    {
        shotAnimator.SetTrigger("ShotTrigger");
    }

    //public void RangedAttackTriggerReset()
    //{
    //    animator.ResetTrigger("BowTrigger");
    //    animator.SetInteger("BowCombo", 0);
    //    IsAttacking = false;
    //}

    //public void AdvanceCombo()
    //{
    //    comboReady = true;

    //    if (attackIndex < maxCombo)
    //        attackIndex++;

    //    UpdateRangedAttackUI(attackIndex - 1);
    //}

    //public void PlayComboEffect()
    //{
    //    isSkill = true;
    //    IsAttacking = true;
    //    comboBar.PlayEffect();
    //    StartCoroutine(InputTimer(0.5f));
    //}

    //IEnumerator InputTimer(float time)
    //{
    //    player.canAttack = false;

    //    yield return new WaitForSeconds(time);

    //    player.canAttack = true;
    //}
}
