using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxCombo = 6;

    private ComboAttackSO comboData;

    public int attackIndex = 0;
    bool canNextCombo = false;
    bool inputCombo = false;

    public bool IsAttacking { get; private set; } = false;

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
            Debug.LogWarning("comboData가 설정되지 않았습니다. 무기가 제대로 장착되었는지 확인하세요.");
            return;
        }

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
        IsAttacking = true;
        attackIndex = 1;
        animator.ResetTrigger("AttackTrigger");
        animator.SetTrigger("AttackTrigger");
        animator.SetInteger("AttackCombo", attackIndex);
        UpdateComboAttackUI(attackIndex - 1);
    }


    void OnComboCheck(float bufferTime)
    {
        canNextCombo = true;
        //DamageTextManager.Instance.ShowComboTiming(bufferTime);
        StartCoroutine(ComboInputBuffer(bufferTime));
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
        if (inputCombo && attackIndex < maxCombo)
        {
            inputCombo = false;
            attackIndex++;
            animator.SetInteger("AttackCombo", attackIndex);
            UpdateComboAttackUI(attackIndex - 1);
            animator.Play(comboData.comboSteps[attackIndex - 1].animationName);
        }
        else
        {
            EndComboAttack();
        }
    }

    public void EndComboAttack()
    {
        if (this == null || animator == null || !gameObject.activeInHierarchy)
            return;

        IsAttacking = false;
        canNextCombo = false;

        if (inputCombo)
        {
            inputCombo = false;
            StartCoroutine(RestartComboAfterDelay(0.1f));
        }
        else
        {
            attackIndex = 0;
            animator.SetInteger("AttackCombo", 0);
            animator.Play("Idle");
            UpdateComboAttackUI(attackIndex);
        }
    }

    private IEnumerator RestartComboAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attackIndex = 1;
        IsAttacking = true;
        canNextCombo = true;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(comboData.comboSteps[attackIndex - 1].animationName))
        {
            animator.ResetTrigger("AttackTrigger");
            animator.SetTrigger("AttackTrigger");
            animator.SetInteger("AttackCombo", attackIndex);
            UpdateComboAttackUI(attackIndex - 1);
        }
    }

    void OnAttackReset()
    {
        EndComboAttack();
    }

    void OnMoveForward(float distance)
    {
        StartCoroutine(MoveForwardCoroutine(distance));
    }

    IEnumerator MoveForwardCoroutine(float distance)
    {
        float moveTime = 0.1f; // 이동 시간 (0.1초 추천)
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = transform.position + (transform.right * distance);

        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveTime);
            yield return null;
        }

        transform.position = targetPos; // 마지막 위치 보정
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

        // 점프 방향 (대각선 위)
        Vector3 jumpDir = (transform.right + Vector3.up*0.5f).normalized;
        Vector3 peakPos = startPos + jumpDir * height;

        Vector3 horizontalDir = transform.right;
        Vector3 endPos = startPos + horizontalDir * height; // 수평 이동 포함

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        LayerMask wallMask = LayerMask.GetMask("Wall", "Ground"); // 충돌 감지할 레이어 설정

        // 상승
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easeT = Mathf.Sin(t * Mathf.PI * 0.5f); // EaseOutSine

            Vector3 nextPos = Vector3.Lerp(startPos, peakPos, easeT);
            Vector3 moveDir = nextPos - transform.position;

            // 벽 체크
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir.normalized, moveDir.magnitude, wallMask);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                yield break; // 점프 중단
            }

            transform.position = nextPos;
            yield return null;
        }

        // 하강
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easeT = 1f - Mathf.Cos(t * Mathf.PI * 0.5f); // EaseInSine

            Vector3 nextPos = Vector3.Lerp(peakPos, endPos, easeT);
            Vector3 moveDir = nextPos - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir.normalized, moveDir.magnitude, wallMask);
            if (hit.collider != null)
            {
                transform.position = hit.point;
                yield break; // 점프 중단
            }

            transform.position = nextPos;
            yield return null;
        }

        transform.position = endPos;
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
                var enemy = target.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.GetDamage(result.damage);

                    Vector3 textPosition = enemy.transform.position + Vector3.up * 1f;
                    DamageTextManager.Instance.ShowDamage(textPosition, (int)result.damage, result.isCrit);
                    GameManager.Instance.cameraShake.Shake(0.2f, 0.4f);
                    Vector2 attackerPos = (Vector2)transform.position + Vector2.up * 0.5f;
                    KnockbackUtil.ApplyKnockback(target, attackerPos, 2f);
                }

                var laber = target.GetComponentInChildren<LaberDamagerble>();
                if (laber != null)
                {
                    Debug.Log("레버 타격!");
                    laber.GetDamage(result.damage);
                }
            }
        }
    }

    private List<GameObject> FindTargetInFront()
    {
        var step = comboData.comboSteps[attackIndex-1];

        float radius = step.radius;
        float offset = step.offset;
        
        Vector2 forward = GameManager.Instance.player.controller.isFacingRight ? Vector2.right : Vector2.left;
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

            return closest != null ? new List<GameObject> { closest } : new List<GameObject>();
        }
        else
        {
            // 3~6타: 관통 공격
            return hits.Select(hit => hit.gameObject).ToList();
        }
    }

    private void UpdateComboAttackUI(int stepIndex)
    {
        if (comboData != null && stepIndex >= 0 && stepIndex < comboData.comboSteps.Count)
        {
            Sprite newIcon = comboData.comboSteps[stepIndex].stepIcon;
            Debug.Log($"UpdateComboAttackUI - 현재 아이콘: {newIcon.name}, 현재 장착 무기: {comboData.name}");
            if (SystemManager.Instance.skillManager.skillUI != null)
            {
                SystemManager.Instance.skillManager.skillUI.SetSkillIcon(SkillSlotType.Basic, newIcon);
            }
        }
    }
}