using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float duration = 2f;
    public float range = 5f;

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
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamagable>(out var damageable))
            {
                var data = BasicAttackData.Create(
                caster: caster,                      // 발사 주체 (플레이어)
                target: other.gameObject,               // 맞은 대상
                comboMultiplier: comboMultiplier// 타수에 따른 계수
                );

                float damage = data.CalculateDamage();
                damageable.GetDamage(damage);
                CameraShake.Instance.Shake(0.1f, 0.2f);
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
        // 풀 반환
        ProjectilePool.Instance.Release(gameObject);
    }
}
