using UnityEngine;

// 동작 안됨
public class ReflectionShot: Bolt
{
    public int reflectionCount = 2;
    private Vector2 currDirection;
    // private GameObject caster;
    
    protected override void OnEnable()
    {
        currDirection = transform.right;
        currTime = 0;
    }
    
    protected override void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        // base.FixedUpdate();
        if (currTime >= duration) { ProjectileManager.Instance.DestroyProjectile(gameObject); }
        
        rigidbody.velocity = currDirection * 3f;
    }
    
     protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Player")) return; // 수정 필요
        
        // Debug.Log(other.gameObject.name); // 프로젝타일 자체도 충돌되는 것으로 확인 함
        Vector3 contactPoint = collision.ClosestPoint(transform.position);
        Vector3 normal = (transform.position - contactPoint).normalized;
    
        Vector3 reflectedDirectino = Vector3.Reflect(rigidbody.velocity.normalized, normal);
        currDirection = reflectedDirectino;
    }        
}