using UnityEngine;

// EnemyDetectHandler - monoBehaviour 아니여도 될 듯
public class EnemyDetectHandler : MonoBehaviour
{
    public enum DetectType { Grounded, Walkable, Blocked }
    
    private EnemyBaseController controller;
    // 컨트롤러로 통일
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private float gravityScale;
    
    private float rayWallDistance = 0.5f;
    private float rayGroundDistance = 0.1f;
    private float groundBoxSize = 0.1f;
    
    private bool isGrounded = false;
    public bool isWalkable { get; private set; } = true;
    public bool isWall { get; private set; } = false;

    
    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        
        rigidbody = GetComponent<Rigidbody2D>(); // 중복 참조 발생 - 컨트롤러 자체를 참조하도록 변경하기
        collider = GetComponent<Collider2D>();
        
        gravityScale = rigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        bool currIsWalkable = IsWalkable();
        if (isWalkable != IsWalkable())
        {
            isWalkable = currIsWalkable;
            controller.OnDetected(DetectType.Walkable, isWalkable);
        }

        // 즉각적인 해결책으로 이용될 수 없음
        bool currIsWall = IsWall();
        if (isWall != currIsWall)
        {
            isWall = currIsWall;
            controller.OnDetected(DetectType.Blocked, isWall);
        }


        bool currIsGrounded = IsGrounded();
        if (isGrounded != currIsGrounded)
        {
            isGrounded = currIsGrounded;
            controller.OnDetected(DetectType.Grounded, isGrounded);
            // rigidbody.gravityScale = isGrounded ? 0 : gravityScale; // 공중에 있을 때만 중력 개념 적용 - 이동이 멈추면 가속도 붙음
        }

        // Vector3 currSlope = GetSlope();
        // if (currSlope.x != 1)
        // {
        // rigidbody.velocity = new Vector2(0, 0);
        // }
    }

    private void OnDrawGizmos()
    {
        collider = GetComponent<Collider2D>();
        Gizmos.DrawCube(new Vector2(collider.bounds.center.x, collider.bounds.min.y), new Vector2(collider.bounds.size.x, groundBoxSize));
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(collider.bounds.center.x, collider.bounds.min.y), new Vector2(collider.bounds.size.x, groundBoxSize), 0, 1 << LayerMask.GetMask("Ground"));
    }

    // movementHandler를 통해서 같이 적용해야하는 걸까?
    private bool IsWalkable()
    {
        var position = new Vector2(transform.rotation.eulerAngles.y == 0 ? collider.bounds.max.x : collider.bounds.min.x, collider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down,  rayGroundDistance, 1 << LayerMask.NameToLayer("Ground"));
        
        Debug.DrawRay(position, Vector2.down * rayGroundDistance);
        
        return hit.collider;
    }

    private bool IsWall() // 앞쪽의 
    {
        Vector2 startPos = new Vector2(transform.eulerAngles.y == 0 ? collider.bounds.max.x : collider.bounds.min.x, collider.bounds.center.y);
        RaycastHit2D hit = Physics2D.Raycast(startPos, transform.right, rayWallDistance, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(startPos, transform.right * rayWallDistance, Color.yellow);

        return hit.collider;
    }
    
    private Vector3 GetSlope() // 경사 체크 - 현재 경사 정보 전달
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(collider.bounds.center.x, collider.bounds.min.y), Vector2.down, rayWallDistance, ~(1 << gameObject.layer));
        var currDegree = Vector2.Angle(Vector2.up, hit.normal);
        
        
        var currDirection = Vector3.ProjectOnPlane(Vector3.right, hit.normal).normalized;
        
        return currDirection;
        
    }
}
