using UnityEngine;

public class ReflectionShot: Projectile
{
    public int reflectionCount = 2;
    private Vector2 currDirection;

    private GameObject caster;
    
    private void Start()
    {
        currDirection = caster.transform.up;
    }

    private void Update()
    {
        rigidbody.velocity = currDirection * 3f;
    }
    
    private void TriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Player")) return; // 수정 필요
        
        // Debug.Log(other.gameObject.name); // 프로젝타일 자체도 충돌되는 것으로 확인 함
        Vector3 contactPoint = other.ClosestPoint(transform.position);
        Vector3 normal = (transform.position - contactPoint).normalized;
    
        Vector3 reflectedDirectino = Vector3.Reflect(rigidbody.velocity.normalized, normal);
        currDirection = reflectedDirectino;
    }        
}