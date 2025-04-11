using System.Collections;
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
    private bool isAttacking = false;

    public void SetRangedAttackData(RangedAttackSO data)
    {
        rangedData = data;
        attackIndex = 0;
        isAttacking = false;

        if (rangedData != null && rangedData.comboSteps.Count > 0)
        {
            GameObject projectilePrefab = rangedData.comboSteps[0].projectilePrefab;
            projectilePool = new GameObjectPool(projectilePrefab, 10);
        }
    }

    public void HandleAttackInput()
    {
        if (rangedData == null) return;

        if (isAttacking)
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
        isAttacking = true;
        attackIndex = 1;
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
        if (inputCombo && attackIndex < rangedData.comboSteps.Count)
        {
            attackIndex++;
            inputCombo = false;
            PlayRangedAnimation();
        }
        else
        {
            EndRangedAttack();
        }
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

            // 스프레드 로직은 3타에만 적용
            if (step.isSpread)
            {
                float angle = step.spreadAngle * (i - (step.projectileCount - 1) / 2f);
                direction = Quaternion.Euler(0, 0, angle) * baseDirection;
            }

            SpawnProjectile(direction);

            yield return new WaitForSeconds(step.fireDelay);
        }
    }

    void SpawnProjectile(Vector3 direction)
    {
        GameObject projectile = ProjectilePool.Instance.Get(firePoint.position, Quaternion.LookRotation(Vector3.forward, direction));

        PlayerProjectile pp = projectile.GetComponent<PlayerProjectile>();
        if (pp != null)
        {
            pp.Setup(direction); // 방향 세팅
        }
    }

    private void EndRangedAttack()
    {
        isAttacking = false;
        attackIndex = 0;
        inputCombo = false;
        canNextCombo = false;
        animator.Play("Idle");
    }
}
