using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage;
    private Transform owner;
    
    private Collider2D collider;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetOwner(Transform owner)
    {
        this.owner = owner;
    }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    // notice: 오브젝트가 enable 될 때 트리거 인식 안되는 현상 발생
    private void OnEnable()
    {
        collider.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDamagable damagable) || owner == other.transform) return;
        damagable.GetDamage(damage);
    }
}
