using System.Collections;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxCombo = 6;

    private ComboAttackSO comboData;

    int attackIndex = 0;
    bool canNextCombo = false;
    bool inputCombo = false;

    public bool isAttacking { get; private set; } = false;

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

        if (isAttacking)
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
        isAttacking = true;
        attackIndex = 1;
        animator.SetTrigger("AttackTrigger");
        animator.SetInteger("AttackCombo", attackIndex);
    }


    void OnComboCheck(float bufferTime)
    {
        canNextCombo = true;
        StartCoroutine(ComboInputBuffer(bufferTime));
    }

    IEnumerator ComboInputBuffer(float time)
    {
        yield return new WaitForSeconds(time);
        canNextCombo = false;
    }

    void OnComboNext()
    {
        if (inputCombo && attackIndex < maxCombo)
        {
            inputCombo = false;
            attackIndex++;
            animator.SetInteger("AttackCombo", attackIndex);
            animator.Play(comboData.comboSteps[attackIndex - 1].animationName);
        }
        else
        {
            EndComboAttack();
        }
    }

    private void EndComboAttack()
    {
        isAttacking = false;
        attackIndex = 0;
        animator.SetInteger("AttackCombo", 0);
        inputCombo = false;
        canNextCombo = false;
        animator.Play("Idle");
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

        // 대각선 방향으로 점프 (오른쪽 + 위 방향)
        Vector3 jumpDir = (transform.right + Vector3.up).normalized;
        Vector3 peakPos = startPos + jumpDir * height;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // 상승
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easeT = Mathf.Sin(t * Mathf.PI * 0.5f); // EaseOutSine
            transform.position = Vector3.Lerp(startPos, peakPos, easeT);
            yield return null;
        }

        // 하강: 시작점 기준으로 대각선 아래로 내려오게
        Vector3 endPos = startPos + transform.right * height; // 수평 이동 포함
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            float easeT = 1f - Mathf.Cos(t * Mathf.PI * 0.5f); // EaseInSine
            transform.position = Vector3.Lerp(peakPos, endPos, easeT);
            yield return null;
        }

        transform.position = endPos;
    }

    void OnAttackHit()
    {
        Debug.Log("onattackhit호출");
        if (comboData == null || attackIndex <= 0 || attackIndex > comboData.comboSteps.Count)
        {
            Debug.LogWarning("잘못된 attackIndex 또는 comboData 없음");
            return;
        }

        float multiplier = comboData.comboSteps[attackIndex - 1].damageMultiplier;

        // 타겟을 찾는 방법은 아래에서 설명
        GameObject target = FindTargetInFront();
        if (target == null) Debug.Log("타겟이 널입니다.");

        var attackData = BasicAttackData.Create(
            caster: gameObject,
            target: target,
            comboMultiplier: multiplier
        );

        float damage = attackData.CalculateDamage();
        Debug.Log($"[근접] {attackIndex}타 데미지: {damage}");
        
        if (target != null)
        {
            var enemy = target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log("데미지 계산 실행");
                enemy.GetDamage(damage);
                CameraShake.Instance.Shake(0.05f, 0.1f);

                Vector2 attackerPos = (Vector2)transform.position + Vector2.up * 0.5f;
                KnockbackUtil.ApplyKnockback(target, attackerPos, 2f);
            }
        }
    }

    private GameObject FindTargetInFront()
    {
        var step = comboData.comboSteps[attackIndex-1];

        float radius = step.radius;
        float angle = step.angle;
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.5f;
        Vector2 forward = transform.right;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius, LayerMask.GetMask("Enemy"));

        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - origin).normalized;
            float dist = Vector2.Distance(origin, hit.transform.position);

            // 마지막 타수면 angle 체크 없이 그냥 원형 판정
            if (attackIndex == comboData.comboSteps.Count || Vector2.Angle(forward, toTarget) <= angle / 2f)
            {
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = hit.gameObject;
                }
            }
        }

        // 디버그 시각화: 막타는 원형, 나머지는 부채꼴
        if (attackIndex == comboData.comboSteps.Count)
            DebugDrawUtil.DrawCircle(origin, radius, Color.yellow);
        else
            DebugDrawUtil.DrawFan(origin, radius, angle, forward, Color.cyan);

        return closest;
    }
}