using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IDamagable damagable)) return;
        damagable.TakeDamage(damage);
    }
}
