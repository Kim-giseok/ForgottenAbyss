using UnityEngine;
using UnityEngine.InputSystem;

public class RangedAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float maxChargeTime = 2.0f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private float chargeTimer = 0f;
    private bool isCharging = false;

    private RangedAttackSO rangedAttackData;

    public void SetRangedAttackData(RangedAttackSO data)
    {
        rangedAttackData = data;
    }

    void Update()
    {
        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            if (chargeTimer >= 0.5f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Charge_Loop"))
            {
                animator.Play("Charge_Loop");
            }
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartCharge();
        }
        else if (context.canceled)
        {
            ReleaseCharge();
        }
    }

    private void StartCharge()
    {
        isCharging = true;
        chargeTimer = 0f;
        animator.Play("Charge_Start");
        Debug.Log("차지 시작");
    }

    private void ReleaseCharge()
    {
        isCharging = false;
        animator.Play("Charge_End");

        float power = Mathf.Clamp01(chargeTimer / maxChargeTime); // 0~1 비율
        FireProjectile(power);
        Debug.Log($"차지 해제: 파워 {power}");
    }

    private void FireProjectile(float power)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * Mathf.Lerp(10f, 30f, power); // 차지 정도에 따라 속도 차등
    }
}
