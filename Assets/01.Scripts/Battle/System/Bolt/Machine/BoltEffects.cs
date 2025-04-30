using UnityEngine;

// 횟수는 어디서 관리? - 컨텍스트 개념 추가하기
// 특정 위치에서 튕기지 않는 현상 발생
public class ReflectionEffect : BoltEffect
{
    public override void Execute(Collider2D collision)
    {
        Vector3 contactPoint = collision.ClosestPoint(bolt.transform.position);
        Vector3 normal = (bolt.transform.position - contactPoint).normalized;
    
         bolt.SetDirection(Vector3.Reflect(bolt.Rigidbody.velocity.normalized, normal));
    }
}

// 관통 대상 지정 필요: 컨트롤러 또는 floor
public class PiercingEffect : BoltEffect
{
    public override void Execute(Collider2D other) { }
}