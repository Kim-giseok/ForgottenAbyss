using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform firePoint;

    private GameObjectPool projectilePool;
    private RangedAttackSO rangedData;
    private int attackIndex = 0;
    private bool canNextCombo = false;
    private bool inputCombo = false;

    public bool IsAttacking { get; private set; } = false;

    public void SetRangedAttackData(RangedAttackSO data)
    {
        rangedData = data;
        attackIndex = 0;
        IsAttacking = false;
    }

    public void HandleAttackInput()
    {
        if (rangedData == null) return;

        if (IsAttacking)
        {
            if (canNextCombo)
                inputCombo = true;
        }
        else
        {
            StartRangedAttack();
        }
    }

    private void StartRangedAttack()
    {
        IsAttacking = true;
        attackIndex = 1;
        animator.ResetTrigger("BowTrigger");
        animator.SetTrigger("BowTrigger");
        animator.SetInteger("BowCombo", attackIndex);
        PlayRangedAnimation();
    }

    private void PlayRangedAnimation()
    {
        string animName = rangedData.comboSteps[attackIndex - 1].animationName;
        animator.Play(animName);
    }

    public void OnRangedCheck(float bufferTime)
    {
        canNextCombo = true;
        StartCoroutine(RangedInputBuffer(bufferTime));
    }

    IEnumerator RangedInputBuffer(float time)
    {
        yield return new WaitForSeconds(time);
        canNextCombo = false;
    }

    public void OnRangedNext()
    {
        Debug.Log($"[Ranged] OnRangedNext called. inputCombo: {inputCombo}, attackIndex: {attackIndex}");


        if (inputCombo && attackIndex < rangedData.comboSteps.Count)
        {
            attackIndex++;
            Debug.Log("attackindec ++");
            inputCombo = false;
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
        if (rangedData == null || rangedData.comboSteps.Count < attackIndex) return;

        RangedComboStep step = rangedData.comboSteps[attackIndex - 1];

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
                onKnockBack(0.2f);
            }

            SpawnProjectile(direction);

            yield return new WaitForSeconds(step.fireDelay);
        }
    }

    void onKnockBack(float distance)
    {
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

        transform.position = targetPos; // 마지막 위치 보정
    }

        void SpawnProjectile(Vector3 direction)
    {
        GameObject projectile = ProjectilePool.Instance.Get(firePoint.position, Quaternion.LookRotation(Vector3.forward, direction));

        PlayerProjectile pp = projectile.GetComponent<PlayerProjectile>();
        if (pp != null)
        {
            pp.Setup(direction, caster: this.gameObject, rangedData.comboSteps[attackIndex-1].multiplier); // 방향 세팅
        }
    }

    public void EndRangedAttack()
    {
        IsAttacking = false;
        attackIndex = 0;
        inputCombo = false;
        canNextCombo = false;
        animator.Play("Idle");
    }
}
