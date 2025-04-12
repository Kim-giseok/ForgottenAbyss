using UnityEngine;

// EnemyDetectHandler
public class EnemyDetectHandler : MonoBehaviour
{
    private float defaultRayDistance = 1f;

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private float gravityScale;
    
    private bool isGrounded = false;
    public bool isWalkable { get; private set; } = true;
    
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>(); // 중복 참조 발생
        collider = GetComponent<Collider2D>();
        
        gravityScale = rigidbody.gravityScale;
    }

    private void Update()
    {
        IsWalkable();
        // if (IsGrounded()) { rigidbody.gravityScale = 0; } else { rigidbody.gravityScale = gravityScale; }
    }

    private void OnDrawGizmos()
    {
        
        collider = GetComponent<Collider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(collider.bounds.center.x, collider.bounds.min.y), new Vector2(collider.bounds.size.x, 0.1f));
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(collider.bounds.center.x, collider.bounds.min.y), new Vector2(collider.bounds.size.x, 0.1f), 0, ~(1 << gameObject.layer));
    }

    // movementHandler를 통해서 같이 적용해야하는 걸까?
    public void IsWalkable()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.rotation.eulerAngles.y == 0 ? collider.bounds.max.x : collider.bounds.min.x, collider.bounds.min.y), Vector2.down, defaultRayDistance, ~(1 << gameObject.layer));
        Debug.DrawRay(new Vector2(transform.rotation.eulerAngles.y == 0 ? collider.bounds.max.x : collider.bounds.min.x, collider.bounds.min.y), Vector2.down * 1f, Color.yellow);
    }
    
    public void IsBlocked() // 앞쪽의 
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, defaultRayDistance, ~(1 << gameObject.layer));
        Debug.DrawRay(transform.position, transform.right * defaultRayDistance, Color.red);
    }
    
    private void IsSlope() // 경사 체크
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(collider.bounds.center.x, collider.bounds.min.y), Vector2.down, defaultRayDistance, ~(1 << gameObject.layer));
        var currDegree = Vector2.Angle(Vector2.up, hit.normal);
        var currDirection = Vector3.ProjectOnPlane(Vector3.right, hit.normal).normalized;
    }
}
