using System.Collections;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxCombo = 6;

    int attackIndex = 0;
    bool canNextCombo = false;
    bool inputCombo = false;

    public bool isAttacking { get; private set; } = false;

    public void Init(Animator animator)
    {
        this.animator = animator;
    }

    public void HandleAttackInput()
    {
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
            animator.Play("SwordAttack_" + attackIndex);
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
}