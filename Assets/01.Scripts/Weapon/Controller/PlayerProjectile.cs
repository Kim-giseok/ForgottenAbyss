using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float duration = 2f;
    public float range = 5f;
    [SerializeField] private LayerMask targetLayer;
    private float timer;
    private float comboMultiplier = 1f;
    private GameObject caster;
    private Rigidbody2D rb;
    private Vector3 startPosition;
    private float attackIndex;
    private GameObject firstHitTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        firstHitTarget = null;
        timer = 0f;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            ReturnToPool();
        }

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (firstHitTarget == null)
            {
                if (other.TryGetComponent<IDamagable>(out var damageable))
                {
                    firstHitTarget = other.gameObject;

                    var data = BasicAttackData.Create(
                    caster: caster,
                    target: firstHitTarget,
                    comboMultiplier: comboMultiplier
                    );

                    var result = data.CalculateDamage();

                    if (other.TryGetComponent<LaberDamagerble>(out _))
                    {
                        Debug.Log("레버 타격!");
                        damageable.GetDamage(result.damage);
                    }
                    else
                    {
                        Debug.Log($"damage : {result.damage}");
                        damageable.GetDamage(result.damage);

                        Vector3 textPosition = other.transform.position + Vector3.up * 1f;
                        DamageTextManager.Instance.ShowDamage(textPosition, (int)result.damage, result.isCrit);

                        GameManager.Instance.cameraShake.Shake(0.1f, 0.2f);

                        if (attackIndex == 3)
                        {
                            Vector2 attackerPos = (Vector2)transform.position + Vector2.up * 0.5f;
                            //KnockbackUtil.ApplyKnockback(other.gameObject, attackerPos, 1.5f);
                        }
                    }
                }

                ReturnToPool();
            }          
        }
        else if (other.CompareTag("Ground")) ReturnToPool();
    }

    public void Setup(Vector3 direction, GameObject caster, float multiplier, float index)
    {
        this.caster = caster;
        comboMultiplier = multiplier;
        rb.velocity = direction.normalized * speed;
        attackIndex = index;
    }

    private void ReturnToPool()
    {
        // Ǯ ��ȯ
        SystemManager.Instance.projectile.Release(gameObject);
    }
}
