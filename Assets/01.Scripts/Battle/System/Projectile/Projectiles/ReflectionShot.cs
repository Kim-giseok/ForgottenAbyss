using UnityEngine;

public class ReflectionShot: MonoBehaviour
{
    public int reflectionCount = 2;
    
    private void Start()
    {
        // reflectDir = projectile.transform.up;
        // Debug.Log(reflectDir);
    }

    private void Update()
    {
        // projectile.rigidbody.velocity = projectile.transform.up * projectile.speed;
        // Debug.Log(reflectDir);
        // projectile.rigidbody.velocity = reflectDir * projectile.speed;
    }

    // public override void TriggerEnter(Collider2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Player")) return; // 수정 필요
    //     
    //     // Debug.Log(other.gameObject.name); // 프로젝타일 자체도 충돌되는 것으로 확인 함
    //     
    //     Vector3 contactPoint = other.ClosestPoint(projectile.transform.position);
    //     Vector3 normal = (projectile.transform.position - contactPoint).normalized;
    //
    //     Vector3 reflected = Vector3.Reflect(projectile.rigidbody.velocity.normalized, normal);
    //     reflectDir= reflected;
    // }        
}