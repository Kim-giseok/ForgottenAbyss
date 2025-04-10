using Unity.Mathematics;
using UnityEngine;

// 확장 메서드 활용?
public class ProjectileBuilder: MonoBehaviour
{
    private Rigidbody2D rigidbody;

    private Transform target;
    private LayerMask targetLayer;
    
    public void SetStraight()
    {
        rigidbody.velocity = new Vector2(3f, rigidbody.velocity.y);
    }

    public void SetGuide() // notice: invoke에 추가하는 등의 방식으로 처리
    {
        if (!target)
        {
            var hit = Physics2D.OverlapCircleAll(transform.position, 1f, targetLayer);
            if (hit == null) return;
            
        }
        
        float degree = ProjectileManager.Instance.GetDegreeByDirection((target.position - transform.position).normalized);
        transform.localRotation = quaternion.Euler(0, 0, degree);
    }
}