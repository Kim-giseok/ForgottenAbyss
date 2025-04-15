using UnityEngine;

public class PiercingShot: Projectile
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currTime >= duration) { ProjectileManager.Instance.DestroyProjectile(gameObject); }
    }

    // 충돌 시 삭제되는 점 제외
    protected override void OnTriggerEnter2D(Collider2D collision) { }
}