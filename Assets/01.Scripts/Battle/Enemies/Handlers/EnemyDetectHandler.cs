using UnityEngine;

// EnemyDetectHandler
public class EnemyDetectHandler : MonoBehaviour
{
    public enum DetectType { Grounded }
    
    private float rayWallDistance = 1f;
    private float rayGroundDistance = 0.2f;

    private EnemyBaseController controller;
    // 컨트롤러로 통일
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private float gravityScale;
    
    private bool isGrounded = false;
    public bool isWalkable { get; private set; } = true;
    
    
    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        
        rigidbody = GetComponent<Rigidbody2D>(); // 중복 참조 발생 - 컨트롤러 자체를 참조하도록 변경하기
        collider = GetComponent<Collider2D>();
        
        gravityScale = rigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        if (isWalkable != IsWalkable())
        {
            isWalkable = IsWalkable();
            // notify
        }
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
    public bool IsWalkable()
    {
        var position = new Vector2(transform.rotation.eulerAngles.y == 0 ? collider.bounds.max.x : collider.bounds.min.x, collider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down,  rayGroundDistance, ~(1 << gameObject.layer));
        
        Debug.DrawRay(position, Vector2.down * rayGroundDistance, Color.yellow);
        
        return hit.collider;
    }
    
    public void IsBlocked() // 앞쪽의 
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayWallDistance, ~(1 << gameObject.layer));
        Debug.DrawRay(transform.position, transform.right * rayWallDistance, Color.red);
    }
    
    private void IsSlope() // 경사 체크
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(collider.bounds.center.x, collider.bounds.min.y), Vector2.down, rayWallDistance, ~(1 << gameObject.layer));
        var currDegree = Vector2.Angle(Vector2.up, hit.normal);
        var currDirection = Vector3.ProjectOnPlane(Vector3.right, hit.normal).normalized;
    }
}
