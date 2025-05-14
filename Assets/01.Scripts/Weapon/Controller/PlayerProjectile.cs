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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
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
            if (other.TryGetComponent<IDamagable>(out var damageable))
            {
                var data = BasicAttackData.Create(
                caster: caster,                      // �߻� ��ü (�÷��̾�)
                target: other.gameObject,               // ���� ���
                comboMultiplier: comboMultiplier// Ÿ���� ���� ���
                );

                var result = data.CalculateDamage();

                if (other.TryGetComponent(out EnemyController enemyController)) {
                    
                    enemyController.GetDamageByType(result.damage, EnemyStatusHandler.HitType.Normal);

                    Vector3 textPosition = other.transform.position + Vector3.up * 1f;
                    DamageTextManager.Instance.ShowDamage(textPosition, (int)result.damage, result.isCrit);
                    GameManager.Instance.cameraShake.Shake(0.1f, 0.6f);
                }

                if(other.GetComponent<LaberDamagerble>() != null)
                {
                    damageable.GetDamage(result.damage);
                }
            }
            ReturnToPool();
        }
        else if (other.CompareTag("Ground")) ReturnToPool();
    }

    public void Setup(Vector3 direction, GameObject caster, float multiplier)
    {
        this.caster = caster;
        comboMultiplier = multiplier;
        rb.velocity = direction.normalized * speed;
    }

    private void ReturnToPool()
    {
        // Ǯ ��ȯ
        SystemManager.Instance.projectile.Release(gameObject);
    }
}
