using System;
using UnityEngine;

// EnemyDetectHandler
public class EnemyDetectHandler : MonoBehaviour
{
    private float RAY_DISTANCE = 1f;
    private float memoGravityScale;

    private Rigidbody2D rigidbody;
    private CircleCollider2D collider;
    
    private LayerMask currLayerMask;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        
        memoGravityScale = rigidbody.gravityScale;
    }

    private void Update()
    {
        IsGrounded();
    }

    public void IsGrounded()
    {
        var ad  =Physics2D.OverlapCircle(new Vector2(this.transform.position.x, this.transform.position.y - collider.radius), RAY_DISTANCE);
        // Debug.Log(ad);
    }

    public void IsBlocked() // 앞쪽의 
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, RAY_DISTANCE, ~(1 << currLayerMask));
    }
    
    private void IsSlope()
    {
        Vector2 rayStart = new Vector2(transform.position.x, transform.position.y - (collider.radius + 0.2f));
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, RAY_DISTANCE);

        if (hit.collider)
        {
            if(hit.collider.gameObject.CompareTag("Player")) return; // Ground 레이어 마스크로 변경
            var currDegree = Vector2.Angle(Vector2.up, hit.normal);
            if(currDegree == 0) rigidbody.gravityScale = 0;
            
        }
    }
}
