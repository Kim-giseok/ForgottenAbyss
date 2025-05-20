using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxCombo = 6;
    [SerializeField] private ComboBar comboBar;

    private ComboAttackSO comboData;
    private ControllerPlayer player;

    public int attackIndex = 0;
    public bool canJumpAttack = true;
    bool canNextCombo = false;
    bool inputCombo = false;
   
    public bool IsAttacking = false;

    private void Start()
    {
        StartCoroutine(WaitForGameManagerReady());
    }

    private IEnumerator WaitForGameManagerReady()
    {
        yield return new WaitUntil(() => GameManager.Instance != null && GameManager.Instance.player.controller != null);
        player = GameManager.Instance.player.controller;
    }

    public void SetComboData(ComboAttackSO comboData)
    {
        this.comboData = comboData;
        maxCombo = comboData != null ? comboData.comboSteps.Count : 0;
    }

    public void Init(Animator animator)
    {
        this.animator = animator;
    }

    public void HandleAttackInput()
    {
        if (comboData == null)
        {
            Debug.LogWarning("comboData가 존재하지 않습니다.");
            return;
        }

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
                inputCombo = true;
        }
        else
        {
            StartComboAttack();
        }
    }

    private void StartComboAttack()
    {
        canJumpAttack = false;
        IsAttacking = true;
        attackIndex = 1;
        animator.ResetTrigger("AttackTrigger");
        animator.SetTrigger("AttackTrigger");
        animator.SetInteger("AttackCombo", attackIndex);
        UpdateComboAttackUI(attackIndex - 1);
    }

    private void StartJumpAttack()
    {
        if (canJumpAttack == true)
        {
            canJumpAttack = false;
            IsAttacking = true;
            attackIndex = 1;
            animator.ResetTrigger("SwordJumpTrigger");
            animator.SetTrigger("SwordJumpTrigger");
        }
        else
            Debug.Log("이미 점프 공격 실행 중...");
    }

    void OnEndJumpAttack()
    {
        canJumpAttack = true;
        IsAttacking = false;
        attackIndex = 1;
    }

    void OnComboCheck(float bufferTime)
    {
        canNextCombo = true;

        comboBar.StartCombo(bufferTime);
        StartCoroutine(ComboInputBuffer(bufferTime));
    }

    void onPlaySound()
    {
        if(attackIndex < 4)
            SoundManager.Instance.Playsfx($"SwordAttack3");
        else
            SoundManager.Instance.Playsfx($"SwordAttack1");
    }

    IEnumerator ComboInputBuffer(float time)
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;

            if (inputCombo)
            {
                OnComboNext();
                yield break;
            }

            yield return null;
        }

        canNextCombo = false;
    }

    void OnComboNext()
    {
        if (player == null) return;

        if (inputCombo)
        {
            if (attackIndex < maxCombo)
            {
                inputCombo = false;
                attackIndex++;
                animator.SetInteger("AttackCombo", attackIndex);
                UpdateComboAttackUI(attackIndex - 1);
                animator.Play(comboData.comboSteps[attackIndex - 1].animationName);
                comboBar.PlayEffect();
            }
            else
            {
                EndComboAttack();
            }
        }
        else
            EndComboAttack();
    }

    public void EndComboAttack()
    {
        if (this == null || animator == null || !gameObject.activeInHierarchy)
            return;

        canJumpAttack = true;
        IsAttacking = false;
        canNextCombo = false;

        if (inputCombo)
        {
            inputCombo = false;
            StartCoroutine(RestartComboAfterDelay(0.1f));
        }
        else
        {
            attackIndex = 1;
            animator.SetInteger("AttackCombo", 0);
            animator.Play("Idle");

            if (SystemManager.Instance.weaponManager.GetCurrentWeaponData()?.Type == WeaponType.Sword)
                UpdateComboAttackUI(attackIndex-1);
        }
    }

    private IEnumerator RestartComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackIndex = 1;
        canJumpAttack = true;
        IsAttacking = true;
        canNextCombo = true;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(comboData.comboSteps[attackIndex - 1].animationName))
        {
            animator.ResetTrigger("AttackTrigger");
            animator.SetTrigger("AttackTrigger");
            animator.SetInteger("AttackCombo", attackIndex);
            comboBar.PlayEffect();
            UpdateComboAttackUI(attackIndex - 1);
        }
    }

    void OnAttackReset()
    {
        EndComboAttack();
    }

    void OnMoveForward(float distance)
    {
        if(!player.isGround) return;

        StartCoroutine(MoveForwardCoroutine(distance));
    }

    IEnumerator MoveForwardCoroutine(float distance)
    {
        float moveTime = 0.1f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + (transform.right * distance);

        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveTime);
            yield return null;
        }

        transform.position = targetPos;
    }

    void OnJumpSmash(string data)
    {
        string[] parts = data.Split(',');
        float height = float.Parse(parts[0]);
        float duration = float.Parse(parts[1]);

        StartCoroutine(JumpSmashCoroutine(height, duration));
    }

    private IEnumerator JumpSmashCoroutine(float height, float duration)
    {
        Vector3 startPos = transform.position;

        Vector3 jumpDir = (transform.right + Vector3.up*0.5f).normalized;
        Vector3 peakPos = startPos + jumpDir * height;

        Vector3 horizontalDir = transform.right;
        Vector3 endPos = startPos + horizontalDir * height;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        LayerMask wallMask = LayerMask.GetMask("Wall", "Ground");

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easeT = Mathf.Sin(t * Mathf.PI * 0.5f);

            Vector3 nextPos = Vector3.Lerp(startPos, peakPos, easeT);
            Vector3 moveDir = nextPos - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir.normalized, moveDir.magnitude, wallMask);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                yield break;
            }

            transform.position = nextPos;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easeT = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);

            Vector3 nextPos = Vector3.Lerp(peakPos, endPos, easeT);
            Vector3 moveDir = nextPos - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir.normalized, moveDir.magnitude, wallMask);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                yield break;
            }

            transform.position = nextPos;
            yield return null;
        }

        transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 10f);
    }

    void OnAttackHit()
    {
        if (comboData == null || attackIndex <= 0 || attackIndex > comboData.comboSteps.Count)
        {
            Debug.LogWarning("잘못된 attackIndex 또는 comboData 없음");
            return;
        }

        float multiplier = comboData.comboSteps[attackIndex - 1].damageMultiplier;

        List<GameObject> targets = FindTargetInFront();

        if (targets == null || targets.Count == 0)
        {
            Debug.Log("타겟이 없습니다.");
            return;
        }

        foreach (var target in targets)
        {
            var attackData = BasicAttackData.Create(
                caster: gameObject,
                target: target,
                comboMultiplier: multiplier
            );

            var result = attackData.CalculateDamage();

            if (target != null)
            {
                if (target.TryGetComponent<LaberDamagerble>(out var laber))
                {
                    Debug.Log("레버 타격!");
                    laber.GetDamage(result.damage);
                    continue;
                }

                if (target.TryGetComponent<IDamagable>(out var damageable))
                {
                    damageable.GetDamage(result.damage);

                    Vector3 textPosition = target.transform.position + Vector3.up * 1f;
                    DamageTextManager.Instance.ShowDamage(textPosition, (int)result.damage, result.isCrit);

                    GameManager.Instance.cameraShake.Shake(0.2f, 0.4f);

                    float knockbackStrength = 0f;
                    if (attackIndex >= 4) knockbackStrength = 3f + (attackIndex - 4) * 1f; // 4타 = 3, 5타 = 4, 6타 = 5

                    if (knockbackStrength > 0)
                    {
                        Vector2 attackerPos = (Vector2)transform.position + Vector2.up * 0.5f;
                        KnockbackUtil.ApplyKnockback(target, attackerPos, knockbackStrength);
                    }
                }
            }
        }
    }

    private List<GameObject> FindTargetInFront()
    {
        var step = comboData.comboSteps[attackIndex-1];

        float radius = step.radius;
        float offset = step.offset;
        
        Vector2 forward = player.isFacingRight ? Vector2.right : Vector2.left;
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f + forward * offset; ;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, LayerMask.GetMask("Enemy"));

        if (attackIndex <= 3)
        {
            GameObject closest = null;
            float minDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                float dist = Vector2.Distance(origin, hit.ClosestPoint(origin));
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = hit.gameObject;
                }
            }
            DebugDrawUtil.DrawCircle(origin, radius, Color.red, 0.5f);
            return closest != null ? new List<GameObject> { closest } : new List<GameObject>();
        }
        else
        {
            // 4~6타: 범위 공격
            DebugDrawUtil.DrawCircle(origin, radius, Color.red, 0.5f);
            return hits.Select(hit => hit.gameObject).ToList();
        }
    }

    public void SwapWeapon()
    {
        UpdateComboAttackUI(0);
    }

    private void UpdateComboAttackUI(int stepIndex)
    {
        if (comboData != null && stepIndex >= 0 && stepIndex < comboData.comboSteps.Count)
        {
            Sprite newIcon = comboData.comboSteps[stepIndex].stepIcon;
 
            if (SystemManager.Instance.skillManager.skillUI != null)
            {
                SystemManager.Instance.skillManager.skillUI.SetSkillIcon(SkillSlotType.Basic, newIcon);
            }
        }
    }
}