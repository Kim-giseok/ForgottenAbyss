using UnityEngine;

public class BoltReflectionEffect : BoltEffect // 횟수는 어디서 관리?
{
    public override void Execute(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Player")) return; // 수정 필요
        
        // Debug.Log(other.gameObject.name); // 프로젝타일 자체도 충돌되는 것으로 확인 함
        Vector3 contactPoint = collision.ClosestPoint(controller.transform.position);
        Vector3 normal = (controller.transform.position - contactPoint).normalized;
    
        Vector3 reflectedDirectino = Vector3.Reflect(controller.rigidbody.velocity.normalized, normal);
        
        
        // currDirection = reflectedDirectino;// 이 정보를 전달할 수가 없는 상황
    }
}

public class BoltPiercingEffect : BoltEffect
{
    public override void Execute(Collider2D other)
    {
    }
}