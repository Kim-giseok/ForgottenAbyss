using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage = 1;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamagable damagable)) return;
        damagable.TakeDamage(damage);
    }
}
