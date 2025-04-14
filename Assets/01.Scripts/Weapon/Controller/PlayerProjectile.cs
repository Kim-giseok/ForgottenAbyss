using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float duration = 2f;

    private float timer;
    private float comboMultiplier = 1f;
    private GameObject caster;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
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
                CameraShake.Instance.Shake(0.05f, 0.1f);
            }

            ReturnToPool();
        }
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
