using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float duration = 2f;
    public int damage = 10;

    private float timer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timer = 0f;
        rb.velocity = transform.right * speed;
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
            // 여기에 데미지 처리 로직 삽입 (예시)
            IDamagable damageable = other.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.GetDamage(damage);
            }

            ReturnToPool();
        }
    }

    public void Setup(Vector3 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    private void ReturnToPool()
    {
        // 풀 반환
        ProjectilePool.Instance.Release(gameObject);
    }
}
