using UnityEngine;

// 센서 시스템에 종속되도록 변경 필요
// EnemyDetectHandler - monoBehaviour 아니여도 될 듯
public class EnemyDetectHandler : MonoBehaviour
{
    public enum DetectType { Grounded, Walkable, Blocked }
    
    private EnemyBaseController controller;
    // 컨트롤러로 통일
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private float gravityScale;
    
    private readonly float rayWallDistance = 0.5f;
    private readonly float rayGroundDistance = 1f;
    private readonly float groundBoxSize = 0.1f;
    
    private bool isGrounded = false;
    public bool isWalkable { get; private set; } = true;
    public bool isWall { get; private set; } = false;

    
    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        
        _rigidbody = GetComponent<Rigidbody2D>(); // 중복 참조 발생 - 컨트롤러 자체를 참조하도록 변경하기
        _collider = GetComponent<Collider2D>();
        
        gravityScale = _rigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        bool currIsWalkable = IsWalkable();
        if (isWalkable != currIsWalkable)
        {
            isWalkable = currIsWalkable;
            controller.Machine.Notify();
        }

        // 즉각적인 해결책으로 이용될 수 없음
        bool currIsWall = IsWall();
        if (isWall != currIsWall)
        {
            isWall = currIsWall;
            controller.Machine.Notify();
        }


        bool currIsGrounded = IsGrounded();
        if (isGrounded != currIsGrounded)
        {
            isGrounded = currIsGrounded;
            controller.Machine.Notify();

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
        _collider = GetComponent<Collider2D>();
        Gizmos.DrawCube(new Vector2(_collider.bounds.center.x, _collider.bounds.min.y), new Vector2(_collider.bounds.size.x, groundBoxSize));
    }

    // 플랫폼 체커로 같이 체크
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(_collider.bounds.center.x, _collider.bounds.min.y), new Vector2(_collider.bounds.size.x, groundBoxSize), 0, 1 << LayerMask.GetMask("Ground"));
    }

    // 플랫폼 체커를 통해 좌우 최대 체크해서 비교하는 방식으로 변경하기
    // movementHandler를 통해서 같이 적용해야하는 걸까?
    private bool IsWalkable()
    {
        var position = new Vector2(transform.rotation.eulerAngles.y == 0 ? _collider.bounds.max.x : _collider.bounds.min.x, _collider.bounds.min.y);
        
        // fix: 카메라 콜라이더로 인해 인식 안됬던 부분으로 확인
        LayerMask mask = LayerMask.GetMask("Ground", "IgnoreCollision");
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down,  rayGroundDistance, mask);
        Debug.DrawRay(position, Vector2.down * rayGroundDistance);
        
        return hit.collider;
    }

    // collider box 형태로 변경
    private bool IsWall() // 앞쪽의 
    {
        Vector2 startPos = new Vector2(transform.eulerAngles.y == 0 ? _collider.bounds.max.x : _collider.bounds.min.x, _collider.bounds.center.y);
        RaycastHit2D hit = Physics2D.Raycast(startPos, transform.right, rayWallDistance, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(startPos, transform.right * rayWallDistance, Color.yellow);

        return hit.collider;
    }
    
    private Vector3 GetSlope() // 경사 체크 - 현재 경사 정보 전달
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(_collider.bounds.center.x, _collider.bounds.min.y), Vector2.down, rayWallDistance, ~(1 << gameObject.layer));
        var currDegree = Vector2.Angle(Vector2.up, hit.normal);
        
        
        var currDirection = Vector3.ProjectOnPlane(Vector3.right, hit.normal).normalized;
        
        return currDirection;
        
    }
}
