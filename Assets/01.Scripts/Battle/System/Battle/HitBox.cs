using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage = 1;
    private Transform parent;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IDamagable damagable)) return;
        damagable.GetDamage(damage);
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }
}
